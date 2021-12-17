/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "cwrapper_generator.h"
#include "be_extern.h"

#include "utl_identifier.h"

#include "ace/OS_NS_sys_stat.h"

#include <cstring>
#include <fstream>
#include <sstream>
#include <map>
#include <iostream>

namespace {
	std::string read_template(const char* prefix) {		
		const char* dds_root = ACE_OS::getenv("DDS_ROOT");
		if (!dds_root) {
			ACE_ERROR((LM_ERROR, "The environment variable DDS_ROOT must be set.\n"));
			BE_abort();
		}
		std::string path = dds_root;
		path.append("/dds/idl/");
		path.append(prefix);
		path.append("Template.txt");		
		std::ifstream ifs(path.c_str());
		std::ostringstream oss;
		oss << ifs.rdbuf();
		return oss.str();
	}

	void replaceAll(std::string& s,	const std::map<std::string, std::string>& rep) {
		typedef std::map<std::string, std::string>::const_iterator mapiter_t;
		for (size_t i = s.find("<%"); i < s.length(); i = s.find("<%", i + 1)) {
			size_t n = s.find("%>", i) - i + 2;
			mapiter_t iter = rep.find(s.substr(i + 2, n - 4));
			if (iter != rep.end()) {
				s.replace(i, n, iter->second);
			}
		}
	}

	std::string replaceString(std::string str, const std::string& from, const std::string& to) {
		size_t start_pos = 0;
		while ((start_pos = str.find(from, start_pos)) != std::string::npos) {
			str.replace(start_pos, from.length(), to);
			start_pos += to.length(); // Handles case where 'to' is a substring of 'from'
		}
		return str;
	}
}

cwrapper_generator::cwrapper_generator()
	: header_template_(read_template("CWrapperHeader")), impl_template_(read_template("CWrapperImpl"))
{
}

bool cwrapper_generator::gen_module(AST_Module* node) {
	return true;
}

bool cwrapper_generator::gen_module_end() {
	return true;
}

bool cwrapper_generator::gen_const(UTL_ScopedName* name, bool nestedInInteface, AST_Constant* constant) {
	return true;
}

bool cwrapper_generator::gen_enum(AST_Enum* node, UTL_ScopedName* name, const std::vector<AST_EnumVal*>& contents, const char* repoid) {
	return true;
}

bool cwrapper_generator::gen_typedef(AST_Typedef* node, UTL_ScopedName* name, AST_Type* base, const char* repoid) {
	return true;
}

bool cwrapper_generator::gen_struct(AST_Structure* structure, UTL_ScopedName* name, const std::vector<AST_Field*>& fields, AST_Type::SIZE_TYPE, const char*)
{			
	const std::string scoped_name = scoped(name);
	const std::string short_name = name->last_component()->get_string();
	const std::string scoped_method = replaceString(std::string(scoped_name), std::string("::"), std::string("_"));

	std::map<std::string, std::string> replacements;
	replacements["SCOPED"] = scoped_name;
	replacements["TYPE"] = short_name;
	replacements["SEQ"] = be_global->sequence_suffix().c_str();
	replacements["SCOPED_METHOD"] = scoped_method;

	be_global->header_ << "/////////////////////////////////////////////////\n"
					   << "// " << short_name << " Declaration\n"
					   << "/////////////////////////////////////////////////\n";

	be_global->header_ << "EXTERN_STRUCT_EXPORT " << scoped_method << "Wrapper\n"
					   << "{\n"
					   << declare_struct_fields(fields).c_str() << "\n"
					   << implement_struct_to_native(fields, short_name, scoped_name).c_str()
					   << implement_struct_from_native(fields, short_name, scoped_name).c_str()
					   << implement_struct_release(fields, short_name, scoped_name)
					   << "};\n\n";
	
	if (be_global->is_topic_type(structure)) {
		std::string header = header_template_;
		replaceAll(header, replacements);
		be_global->header_ << header;

		std::string impl = impl_template_;
		replaceAll(impl, replacements);
		be_global->impl_ << impl;
	}
	
	return true;
}

bool cwrapper_generator::gen_union(AST_Union*, UTL_ScopedName* name, const std::vector<AST_UnionBranch*>&, AST_Type*, const char*)
{
	if (idl_global->is_dcps_type(name)) {
		std::cerr << "ERROR: union " << scoped(name) << " can not be used as a "
			"DCPS_DATA_TYPE (only structs can be Topic types)" << std::endl;
		return false;
	}

	// TODO: Implement unions.
	std::cerr << "ERROR: union not implemented yet." << std::endl;

	return true;
}

std::string cwrapper_generator::declare_struct_fields(const std::vector<AST_Field*>& fields) {
	std::string indent("    ");
	std::string ret("");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* type = field->field_type();		
		const char* name = field->local_name()->get_string();		
		std::string type_name = get_cwrapper_type(type);
		std::string array_dec = get_array_definition(type);

		ret.append(indent);
		ret.append(type_name.c_str());
		ret.append(" ");
		ret.append(name);
		ret.append(array_dec);
		ret.append(";\n");
	}

	return ret;
}

std::string cwrapper_generator::implement_struct_to_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name) {
	std::string ret("    ");

	ret.append(scoped_name);
	ret.append(" to_native()\n");
	ret.append("    {\n");
	ret.append("        ");
	ret.append(scoped_name);
	ret.append(" ret;\n\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();

		ret.append(get_field_to_native(field_type, field_name));
	}

	ret.append("\n        return ret;\n");
	ret.append("    }\n\n");

	return ret;
}

std::string cwrapper_generator::implement_struct_from_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name) {
	std::string ret("    void from_native(");
	ret.append(scoped_name);
	ret.append(" native)\n");
	ret.append("    {\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();

		ret.append(get_field_from_native(field_type, field_name));
	}

	ret.append("    }\n\n");

	return ret;
}

std::string cwrapper_generator::implement_struct_release(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name) {
	std::string ret("    void release()\n");	
	ret.append("    {\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();

		ret.append(get_field_release(field_type, field_name));
	}

	ret.append("    }\n\n");

	return ret;
}

std::string cwrapper_generator::get_cwrapper_type(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();	
	std::string ret("");

	switch (node_type)
	{	
	case AST_Decl::NT_union:			
	case AST_Decl::NT_struct:		
	{		
		ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("_"));		
		ret.append("Wrapper");
		break;
	}
	case AST_Decl::NT_enum:	
	{
		ret = type->full_name();
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_cwrapper_type(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		ret = "CORBA::Double";
		break;
	}
	case AST_Decl::NT_string:
	{
		ret = "CORBA::Char*";
		break;
	}
	case AST_Decl::NT_wstring:
	{
		ret = "CORBA::WChar*";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		switch (predefined_type->pt())
		{
		case AST_PredefinedType::PT_short:
			ret = "CORBA::Short";
			break;
		case AST_PredefinedType::PT_long:
			ret = "CORBA::Long";
			break;
		case AST_PredefinedType::PT_longlong:
			ret = "CORBA::LongLong";
			break;
		case AST_PredefinedType::PT_ushort:
			ret = "CORBA::UShort";
			break;
		case AST_PredefinedType::PT_ulong:
			ret = "CORBA::ULong";
			break;
		case AST_PredefinedType::PT_ulonglong:
			ret = "CORBA::ULongLong";
			break;
		case AST_PredefinedType::PT_float:
			ret = "CORBA::Float";
			break;
		case AST_PredefinedType::PT_double:
			ret = "CORBA::Double";
			break;
		case AST_PredefinedType::PT_longdouble:
			ret = "CORBA::Double";
			break;
		case AST_PredefinedType::PT_octet:
			ret = "CORBA::Octet";			
			break;
		case AST_PredefinedType::PT_char:
			ret = "CORBA::Char";
			break;
		case AST_PredefinedType::PT_wchar:
			ret = "CORBA::WChar";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "CORBA::Boolean";
			break;
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		AST_Expression** dims = arr_type->dims();
		if (arr_type->n_dims() == 1) {
			std::string base_type = get_cwrapper_type(arr_type->base_type());
			ret = base_type;
		}
		else {
			ret = "void*";
		}
		break;
	}		
	case AST_Decl::NT_sequence:
	{
		ret = "void*";
		break;
	}		
	default:
		break;
	}

	return ret;
}

std::string cwrapper_generator::get_array_definition(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();
	std::string ret("");

	switch (node_type)
	{
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		if (arr_type->n_dims() == 1) {
			AST_Expression** dims = arr_type->dims();
			ret.append("[");
			ret.append(std::to_string(dims[0]->ev()->u.ulval));
			ret.append("]");
		}
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_array_definition(typedef_type->base_type());
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string cwrapper_generator::get_field_to_native(AST_Type* type, const char * name) {
	std::string ret("");
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{		
		ret.append("        ret.");
		ret.append(name);
		ret.append(" = ");
		ret.append(name);
		ret.append(".to_native();\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret.append("        if (");
		ret.append(name);
		ret.append(" != NULL)\n");
		ret.append("        {\n");

		ret.append("            ret.");
		ret.append(name);
		if (node_type == AST_Decl::NT_string) {
			ret.append(" = CORBA::string_dup(");
		}
		else {
			ret.append(" = CORBA::wstring_dup(");
		}

		ret.append(name);
		ret.append(");\n");

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_enum:
	{				
		ret.append("        ret.");
		ret.append(name);
		ret.append(" = ");
		ret.append(name);
		ret.append(";\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_to_native(typedef_type->base_type(), name);
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {		
			ret.append("        ret.");
			ret.append(name);
			ret.append(" = ");
			ret.append(name);
			ret.append(";\n");
		}
		else {
			ret.append("#if ACE_SIZEOF_LONG_DOUBLE == 16\n");

			ret.append("printf(\"LINUX TO NATIVE: Value of double before cast = %f\\n\", ");
			ret.append(name);
			ret.append(");\n");

			ret.append("        long double const_");			
			ret.append(name);
			ret.append(" = (long double)");
			ret.append(name);
			ret.append(";\n");

			ret.append("printf(\"LINUX TO NATIVE: Value of long double after cast = %Lf\\n\", const_");
			ret.append(name);
			ret.append(");\n");

			ret.append("        ret.");
			ret.append(name);
			ret.append(" = const_"); 
			ret.append(name);
			ret.append(";\n");

			ret.append("#else\n");

			ret.append("printf(\"WINDOWS TO NATIVE: Value of double before cast = %f\\n\", ");
			ret.append(name);
			ret.append(");\n");

			ret.append("        const long double const_");
			ret.append(name);
			ret.append(" = ");
			ret.append(name);
			ret.append(";\n");

			ret.append("printf(\"WINDOWS TO NATIVE: Value of long double after cast = %Lf\\n\", const_");
			ret.append(name);
			ret.append(");\n");

			ret.append("        ret.");
			ret.append(name);
			ret.append(".assign(const_");
			ret.append(name);
			ret.append(");\n");

			ret.append("#endif\n");
		}
		break;
	}
	case AST_Decl::NT_sequence: 
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(seq_type->base_type());
		AST_Decl::NodeType base_node_type = seq_type->base_type()->node_type();
		unsigned int bound = seq_type->max_size()->ev()->u.ulval;
		std::string sequence_kind = bound > 0 ? "bounded" : "unbounded";
		
		ret.append("        if (");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			ret.append("            TAO::");
			ret.append(sequence_kind);
			ret.append("_value_sequence<");
			ret.append(base_type);			
			if (bound > 0) {
				ret.append(", ");
				ret.append(std::to_string(bound));
			}
			ret.append("> aux;\n");

			ret.append("            marshal::ptr_to_");
			ret.append(sequence_kind);
			ret.append("_sequence(");
			ret.append(name);
			ret.append(", aux);\n");

			ret.append("            ACE_UINT32 length = aux.length();\n");

			ret.append("            ret.");
			ret.append(name);
			ret.append(".length(length);\n");

			ret.append("            for (ACE_UINT32 i = 0; i < length; i++)\n");

			ret.append("            {\n");

			ret.append("                ret.");
			ret.append(name);
			ret.append("[i] = aux[i].to_native();\n");

			ret.append("            }\n");
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			ret.append("            marshal::ptr_to_");
			ret.append(sequence_kind);
			if (base_node_type == AST_Decl::NT_string) {
				ret.append("_basic_string_sequence(");
			}
			else {
				ret.append("_wide_string_sequence(");
			}
			ret.append(name);
			ret.append(", ret.");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_pre_defined:
		{
			AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(seq_type->base_type());

			if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
				ret.append("            TAO::");
				ret.append(sequence_kind);
				ret.append("_value_sequence<CORBA::Double");
				if (bound > 0) {
					ret.append(", ");
					ret.append(std::to_string(bound));					
				}
				ret.append("> aux;\n");

				ret.append("            marshal::ptr_to_");
				ret.append(sequence_kind);
				ret.append("_sequence(");
				ret.append(name);
				ret.append(", aux);\n");

				ret.append("            int length = aux.length();\n");

				ret.append("            ret.");
				ret.append(name);
				ret.append(".length(length);\n");

				ret.append("            for (int i = 0; i < length; i++)\n");

				ret.append("            {\n");

				ret.append("#if ACE_SIZEOF_LONG_DOUBLE == 16\n");

				ret.append("                ret.");
				ret.append(name);
				ret.append("[i] = aux[i];\n");

				ret.append("#else\n");

				ret.append("                ret.");
				ret.append(name);
				ret.append("[i].assign(aux[i]);\n");

				ret.append("#endif\n");
				
				ret.append("            }\n");				
				break;
			}
		}
		default: 
		{
			ret.append("            marshal::ptr_to_");
			ret.append(sequence_kind);
			ret.append("_sequence(");
			ret.append(name);
			ret.append(", ret.");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		}

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		ret.append("        if (!");
		ret.append(name);
		ret.append("\n");

		ret.append("        {\n");

		if (arr_type->n_dims() == 1)
		{
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            for (unsigned int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                ret.");
				ret.append(name);
				ret.append("[i] = ");
				ret.append(name);
				ret.append("[i].to_native();\n");

				ret.append("            }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("            for (unsigned int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                if (");
				ret.append(name);
				ret.append("[i] != NULL)\n");

				ret.append("                {\n");

				ret.append("                    ret.");
				ret.append(name);
				if (base_node_type == AST_Decl::NT_string) {
					ret.append("[i] = CORBA::string_dup(");
				}
				else {
					ret.append("[i] = CORBA::wstring_dup(");
				}
				ret.append(name);
				ret.append("[i]);\n");

				ret.append("                }\n");
				
				ret.append("            }\n");
				break;
			}
			default:
			{
				ret.append("            ACE_OS::memcpy(ret.");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append(");\n");
				break;
			}
			}
		}
		else {
			ACE_UINT32 total_dim = 1;
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				total_dim *= dims[i]->ev()->u.ulval;
			}

			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            ");
				ret.append(base_type);
				ret.append(" arr_");
				ret.append(name);
				ret.append("[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append("            ACE_OS::memcpy(arr_");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n\n");

				ret.append("            int i = 0;\n");

				std::string indent("            ");
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					ret.append(indent);
					ret.append("for (ACE_UINT32 i");
					ret.append(std::to_string(i));
					ret.append(" = 0; i");
					ret.append(std::to_string(i));
					ret.append(" < ");
					ret.append(std::to_string(dims[i]->ev()->u.ulval));
					ret.append("; ++i");
					ret.append(std::to_string(i));
					ret.append(") {\n");

					indent.append("    ");
				}

				ret.append(indent);
				ret.append("ret.");
				ret.append(name);
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					ret.append("[i");
					ret.append(std::to_string(i));
					ret.append("]");
				}
				ret.append(" = arr_");
				ret.append(name);
				ret.append("[i].to_native();\n");

				ret.append(indent);
				ret.append("i++;\n");

				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					indent.erase(0, 4);
					ret.append(indent);
					ret.append("}\n");
				}
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("            ");
				if (base_node_type == AST_Decl::NT_string) {
					ret.append("char");
				}
				else {
					ret.append("wchar_t");
				}
				ret.append("** arr_");
				ret.append(name);
				ret.append(" = new ");
				if (base_node_type == AST_Decl::NT_string) {
					ret.append("char");
				}
				else {
					ret.append("wchar_t");
				}
				ret.append("*[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				if (base_node_type == AST_Decl::NT_string) {
					ret.append("            marshal::ptr_to_basic_string_multi_array(");
				}
				else {
					ret.append("            marshal::ptr_to_wide_string_multi_array(");
				}
				ret.append(name);
				ret.append(", arr_");
				ret.append(name);
				ret.append(", ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            ACE_OS::memcpy(ret.");
				ret.append(name);
				ret.append(", arr_");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append("*) * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            delete[] arr_");
				ret.append(name);
				ret.append(";\n");
				break;
			}
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {					
					std::string indent = "            ";
					ret.append(indent);
					ret.append(base_type);
					ret.append(" aux[");
					ret.append(std::to_string(total_dim));
					ret.append("];\n");

					ret.append(indent);
					ret.append("ACE_OS::memcpy(aux, ");
					ret.append(name);
					ret.append(", sizeof(");
					ret.append(base_type);
					ret.append(") * ");
					ret.append(std::to_string(total_dim));
					ret.append(");\n");

					ret.append(indent);
					ret.append("int i = 0;\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(indent);
						ret.append("for (ACE_UINT32 i");
						ret.append(std::to_string(i));						
						ret.append(" = 0; i");
						ret.append(std::to_string(i));
						ret.append(" < ");
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						ret.append("; ++i");
						ret.append(std::to_string(i));
						ret.append(") {\n");

						indent.append("    ");
					}

					ret.append("#if ACE_SIZEOF_LONG_DOUBLE == 16\n");

					ret.append(indent);
					ret.append("ret.");
					ret.append(name);
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("[i");
						ret.append(std::to_string(i));
						ret.append("]");
					}
					ret.append(" = aux[i];\n");

					ret.append("#else\n");

					ret.append(indent);
					ret.append("ret.");
					ret.append(name);
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("[i");
						ret.append(std::to_string(i));
						ret.append("]");
					}
					ret.append(".assign(aux[i]);\n");

					ret.append("#endif\n");

					ret.append(indent);
					ret.append("i++;\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						indent.erase(0, 4);
						ret.append(indent);
						ret.append("}\n");
					}
					break;
				}
			}
			default:
			{
				ret.append("            ACE_OS::memcpy(ret.");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");
				break;
			}
			}
		}

		ret.append("        }\n");
		break;
	}
	}

	return ret;
}

std::string cwrapper_generator::get_field_from_native(AST_Type* type, const char * name) {
	std::string ret("");
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret.append("        ");
		ret.append(name);
		ret.append(".from_native(native.");
		ret.append(name);
		ret.append(");\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret.append("        if (native.");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		ret.append("            ");
		ret.append(name);
		if (node_type == AST_Decl::NT_string) {
			ret.append(" = CORBA::string_dup(native.");
		}
		else {
			ret.append(" = CORBA::wstring_dup(native.");
		}
		ret.append(name);
		ret.append(");\n");

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_enum:
	{
		ret.append("        ");
		ret.append(name);
		ret.append(" = ");
		ret.append("native.");
		ret.append(name);
		ret.append(";\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_from_native(typedef_type->base_type(), name);
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType* predefined_type = AST_PredefinedType::narrow_from_decl(type);

		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {
			ret.append("        ");
			ret.append(name);
			ret.append(" = native.");
			ret.append(name);
			ret.append(";\n");
		}
		else {
			ret.append("printf(\"FROM NATIVE: Value of long double before cast = %Lf\\n\", native.");
			ret.append(name);
			ret.append(");\n");

			ret.append("        ");
			ret.append(name);
			ret.append(" = static_cast<CORBA::Double>(native.");
			ret.append(name);
			ret.append(");\n");

			ret.append("printf(\"FROM NATIVE: Value of double after cast = %f\\n\", ");
			ret.append(name);
			ret.append(");\n");
		}
		break;
	}
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(seq_type->base_type());
		AST_Decl::NodeType base_node_type = seq_type->base_type()->node_type();
		unsigned int bound = seq_type->max_size()->ev()->u.ulval;
		std::string sequence_kind = bound > 0 ? "bounded" : "unbounded";

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			ret.append("        {\n");

			ret.append("            TAO::");
			ret.append(sequence_kind);
			ret.append("_value_sequence<");
			ret.append(base_type);
			if (bound > 0) {
				ret.append(", ");
				ret.append(std::to_string(bound));
			}
			ret.append("> aux;\n");

			ret.append("            ACE_UINT32 length = native.");
			ret.append(name);
			ret.append(".length();\n");

			ret.append("            aux.length(length);\n");

			ret.append("            for (ACE_UINT32 i = 0; i < length; i++)\n");

			ret.append("            {\n");

			ret.append("                aux[i].from_native(native.");
			ret.append(name);
			ret.append("[i]);\n");

			ret.append("            }\n");

			ret.append("            marshal::");
			ret.append(sequence_kind);
			ret.append("_sequence_to_ptr(aux, ");
			ret.append(name);
			ret.append(");\n");

			ret.append("        }\n");			
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			ret.append("        marshal::");
			ret.append(sequence_kind);
			if (base_node_type == AST_Decl::NT_string) {
				ret.append("_basic_string_sequence_to_ptr(native.");
			}
			else {
				ret.append("_wide_string_sequence_to_ptr(native.");
			}
			ret.append(name);
			ret.append(", ");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_pre_defined:
		{
			AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(seq_type->base_type());

			if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
				ret.append("        {\n");

				ret.append("            int length = native.");
				ret.append(name);
				ret.append(".length();\n");

				ret.append("            TAO::");
				ret.append(sequence_kind);
				ret.append("_value_sequence<CORBA::Double");
				if (bound > 0) {
					ret.append(", ");
					ret.append(std::to_string(bound));
				}
				ret.append("> aux;\n");

				ret.append("            aux.length(length);\n");

				ret.append("            for (int i = 0; i < length; i++)\n");

				ret.append("            {\n");

				ret.append("                aux[i] = native.");
				ret.append(name);
				ret.append("[i];\n");

				ret.append("            }\n");

				ret.append("            marshal::");
				ret.append(sequence_kind);
				ret.append("_sequence_to_ptr(aux, ");
				ret.append(name);
				ret.append(");\n");

				ret.append("        }\n");				
				break;
			}
		}
		default:
		{
			ret.append("        marshal::");
			ret.append(sequence_kind);
			ret.append("_sequence_to_ptr(native.");
			ret.append(name);
			ret.append(", ");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		ret.append("        if (native.");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		if (arr_type->n_dims() == 1)
		{
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            for (unsigned int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                ");
				ret.append(name);
				ret.append("[i].from_native(native.");
				ret.append(name);
				ret.append("[i]);\n");

				ret.append("            }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("            for (unsigned int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                if (native.");
				ret.append(name);
				ret.append("[i] != NULL)\n");

				ret.append("                {\n");

				ret.append("                    ");
				ret.append(name);
				if (base_node_type == AST_Decl::NT_string) {
					ret.append("[i] = CORBA::string_dup(native.");
				}
				else {
					ret.append("[i] = CORBA::wstring_dup(native.");
				}
				ret.append(name);
				ret.append("[i]);\n");

				ret.append("                }\n");

				ret.append("            }\n");
				break;
			}
			default:
			{
				ret.append("            ACE_OS::memcpy(");
				ret.append(name);
				ret.append(", native.");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append(");\n");
				break;
			}
			}
		}
		else {
			ACE_UINT32 total_dim = 1;
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				total_dim *= dims[i]->ev()->u.ulval;
			}

			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            ");
				ret.append(base_type);
				ret.append(" arr_");
				ret.append(name);
				ret.append("[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append("            int i = 0;\n");

				std::string indent("            ");
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					ret.append(indent);
					ret.append("for (ACE_UINT32 i");
					ret.append(std::to_string(i));
					ret.append(" = 0; i");
					ret.append(std::to_string(i));
					ret.append(" < ");
					ret.append(std::to_string(dims[i]->ev()->u.ulval));
					ret.append("; ++i");
					ret.append(std::to_string(i));
					ret.append(") {\n");

					indent.append("    ");
				}

				ret.append(indent);
				ret.append("arr_");
				ret.append(name);
				ret.append("[i].from_native(native.");
				ret.append(name);
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					ret.append("[i");
					ret.append(std::to_string(i));
					ret.append("]");
				}
				ret.append(");\n");

				ret.append(indent);
				ret.append("i++;\n");

				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					indent.erase(0, 4);
					ret.append(indent);
					ret.append("}\n");
				}

				ret.append("            ");
				ret.append(name);
				ret.append(" = ACE_OS::malloc(sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            ACE_OS::memcpy(");
				ret.append(name);
				ret.append(", arr_");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("            ");
				ret.append(base_type);
				ret.append("* arr_");
				ret.append(name);
				ret.append(" = new ");
				ret.append(base_type);
				ret.append("[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append("            ACE_OS::memcpy(arr_");
				ret.append(name);
				ret.append(", native.");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				if (base_node_type == AST_Decl::NT_string) {
					ret.append("            marshal::basic_string_multi_array_to_ptr(arr_");
				}
				else {
					ret.append("            marshal::wide_string_multi_array_to_ptr(arr_");
				}
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(", ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            delete[] arr_");
				ret.append(name);
				ret.append(";\n");
				break;
			}
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
					std::string indent = "            ";
					ret.append(indent);
					ret.append(base_type);
					ret.append(" aux");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("[");
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						ret.append("]");
					}
					ret.append(";\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(indent);
						ret.append("for (ACE_UINT32 i");
						ret.append(std::to_string(i));
						ret.append(" = 0; i");
						ret.append(std::to_string(i));
						ret.append(" < ");
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						ret.append("; ++i");
						ret.append(std::to_string(i));
						ret.append(") {\n");

						indent.append("    ");
					}

					ret.append(indent);
					ret.append("aux");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("[i");
						ret.append(std::to_string(i));
						ret.append("]");
					}
					ret.append(" = native.");
					ret.append(name);
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("[i");
						ret.append(std::to_string(i));
						ret.append("]");
					}
					ret.append(";\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						indent.erase(0, 4);
						ret.append(indent);
						ret.append("}\n");
					}

					ret.append(indent);
					ret.append(name);
					ret.append(" = ACE_OS::malloc(sizeof(");
					ret.append(base_type);
					ret.append(") * ");
					ret.append(std::to_string(total_dim));
					ret.append(");\n");

					ret.append(indent);
					ret.append("ACE_OS::memcpy(");
					ret.append(name);
					ret.append(", aux, sizeof(");
					ret.append(base_type);
					ret.append(") * ");
					ret.append(std::to_string(total_dim));
					ret.append(");\n");
					break;
				}
			}
			default:
			{
				ret.append("            ");
				ret.append(name);
				ret.append(" = ACE_OS::malloc(sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            ACE_OS::memcpy(");
				ret.append(name);
				ret.append(", native.");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");
				break;
			}
			}
		}

		ret.append("        }\n");
		break;
	}
	}

	return ret;
}

std::string cwrapper_generator::get_field_release(AST_Type* type, const char * name) {
	std::string ret("");
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret.append("        ");
		ret.append(name);
		ret.append(".release();\n");		
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret.append("        if (");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		if (node_type == AST_Decl::NT_string) {
			ret.append("            CORBA::string_free(");
		}
		else {
			ret.append("            CORBA::wstring_free(");
		}
		ret.append(name);
		ret.append(");\n");

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_release(typedef_type->base_type(), name);
		break;
	}
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(seq_type->base_type());
		AST_Decl::NodeType base_node_type = seq_type->base_type()->node_type();
		unsigned int bound = seq_type->max_size()->ev()->u.ulval;
		std::string sequence_kind = bound > 0 ? "bounded" : "unbounded";

		ret.append("        if (");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			ret.append("			marshal::release_structure_sequence_ptr<");
			ret.append(base_type);
			ret.append(">(");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			ret.append("            marshal::release_");
			if (base_node_type == AST_Decl::NT_string) {
				ret.append("basic_string_sequence_ptr(");
			}
			else {
				ret.append("wide_string_sequence_ptr(");
			}
			ret.append(name);
			ret.append(");\n");
			break;
		}
		default:
		{
			ret.append("            ACE_OS::free(");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		}

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		ACE_UINT32 total_dim = 1;
		for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
			total_dim *= dims[i]->ev()->u.ulval;
		}

		ret.append("        if (");
		ret.append(name);
		ret.append(" != NULL)\n");

		ret.append("        {\n");

		bool reset = false;
		if (arr_type->n_dims() == 1)
		{
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            for (unsigned int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                ");
				ret.append(name);
				ret.append("[i].release();\n");

				ret.append("            }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("            for (int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                if (");
				ret.append(name);
				ret.append("[i] != NULL)\n");

				ret.append("                {\n");

				if (base_node_type == AST_Decl::NT_string) {
					ret.append("                    CORBA::string_free(");
				}
				else {
					ret.append("                    CORBA::wstring_free(");
				}
				ret.append(name);
				ret.append("[i]);\n");

				ret.append("                }\n");

				ret.append("            }\n");
				break;
			}
			default:
				reset = true;				
				break;
			}
		}
		else {
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("            ");
				ret.append(base_type);
				ret.append(" arr_");
				ret.append(name);
				ret.append("[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append("            ACE_OS::memcpy(arr_");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(", sizeof(");
				ret.append(base_type);
				ret.append(") * ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");

				ret.append("            for (int i = 0; i < ");
				ret.append(std::to_string(total_dim));
				ret.append("; i++)\n");

				ret.append("            {\n");

				ret.append("                arr_");
				ret.append(name);
				ret.append("[i].release();\n");

				ret.append("            }\n");

				ret.append("            ACE_OS::free(");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				if (base_node_type == AST_Decl::NT_string) {
					ret.append("            marshal::release_basic_string_multi_array_ptr(");					
				}
				else {
					ret.append("            marshal::release_wide_string_multi_array_ptr(");
				}
				ret.append(name);
				ret.append(", ");
				ret.append(std::to_string(total_dim));
				ret.append(");\n");
				break;
			}
			default:
				ret.append("            ACE_OS::free(");
				ret.append(name);
				ret.append(");\n");
				break;
			}
		}

		ret.append("        }\n");

		if (reset) {
			ret = "";
		}
		break;
	}
	}

	return ret;
}
