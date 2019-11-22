/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

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
	/*std::string cli_type("");
	
	bool is_string = false;
	bool is_char = false;
	switch (constant->et()) {
	case AST_Expression::EV_short:
		cli_type = "System::Int16";
		break;
	case AST_Expression::EV_ushort:
		cli_type = "System::UInt16";
		break;
	case AST_Expression::EV_long:
		cli_type = "System::Int32";
		break;
	case AST_Expression::EV_ulong:
		cli_type = "System::UInt32";
		break;
	case AST_Expression::EV_float:
		cli_type = "System::Single";
		break;
	case AST_Expression::EV_double:
		cli_type = "System::Double";
		break;
	case AST_Expression::EV_char:
	case AST_Expression::EV_wchar:
		is_char = true;
		cli_type = "System::Char";
		break;
	case AST_Expression::EV_octet:
		cli_type = "System::Byte";
		break;
	case AST_Expression::EV_bool:
		cli_type = "System::Boolean";
		break;
	case AST_Expression::EV_string:
	case AST_Expression::EV_wstring:
		is_string = true;
		cli_type = "System::String^";
		break;
	case AST_Expression::EV_longlong:
		cli_type = "System::Int64";
		break;
	case AST_Expression::EV_ulonglong:
		cli_type = "System::UInt64";
		break;
	case AST_Expression::EV_fixed:
		cli_type = "System::Decimal";
		break;
	default:
		//CODE REVIEW: Error and stop?
		return true;
	}
	
	be_global->header_ << "    public ref class " << name->last_component()->get_string() << " sealed {\n";
	be_global->header_ << "    public:\n";
	be_global->header_ << "        static const " << cli_type << " VALUE = ";

	if (is_string) {
		be_global->header_ << "\"";
	}
	else if (is_char) {
		be_global->header_ << "'";
	}

	constant->constant_value()->dump(be_global->header_);

	if (is_string) {
		be_global->header_ << "\"";
	}
	else if (is_char) {
		be_global->header_ << "'";
	}

	be_global->header_ << ";\n";

	be_global->header_ << "    private:\n";
	be_global->header_ << "        " << name->last_component()->get_string() << "() {}\n";
	be_global->header_ << "    };\n";*/

	return true;
}

bool cwrapper_generator::gen_enum(AST_Enum* node, UTL_ScopedName* name, const std::vector<AST_EnumVal*>& contents, const char* repoid) {

	/*be_global->header_ << "\n    public enum class " << name->last_component()->get_string() << " {\n";

	for (unsigned int i = 0; i < contents.size(); i++) {
		AST_EnumVal* val = contents[i];
		be_global->header_ << "        " << val->local_name()->get_string() << " = ::" << val->full_name();

		if (i + 1 < contents.size()) {
			be_global->header_ << ",";
		} 

		be_global->header_ << "\n";
	}

	be_global->header_ << "    };\n";*/

	return true;
}

bool cwrapper_generator::gen_typedef(AST_Typedef* node, UTL_ScopedName* name, AST_Type* base, const char* repoid) {
	return true;
}

bool cwrapper_generator::gen_struct(AST_Structure*, UTL_ScopedName* name, const std::vector<AST_Field*>& fields, AST_Type::SIZE_TYPE, const char*)
{		
	const std::string scoped_name = scoped(name);
	const std::string short_name = name->last_component()->get_string();
	const std::string scoped_method = replaceString(scoped_name, std::string("::"), std::string("_"));

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

	IDL_GlobalData::DCPS_Data_Type_Info* info = idl_global->is_dcps_type(name);
	if (info) {
		std::string header = header_template_;
		replaceAll(header, replacements);
		be_global->header_ << header;

		std::string impl = impl_template_;
		replaceAll(impl, replacements);
		be_global->impl_ << impl;
	}					   

	/*const std::string scoped_name = scoped(name);
	const std::string short_name = name->last_component()->get_string();

	std::map<std::string, std::string> replacements;
	replacements["SCOPED"] = scoped_name;
	replacements["TYPE"] = short_name;
	replacements["SEQ"] = be_global->sequence_suffix().c_str();	

	be_global->header_ << "\n    public ref class " << short_name << " {\n\n"
					   << "    private:\n"
					   << declare_struct_fields(fields).c_str() << "\n"
					   << "    public:\n"
					   << declare_struct_field_properties(fields).c_str() << "\n"
					   << "    public:\n"
					   << "        " << short_name << "();\n\n"
					   << "    internal:\n"
					   << "        ::" << scoped_name << " ToNative();\n"
					   << "        void FromNative(::" << scoped_name << " native);\n"
					   << "    };\n\n";

	be_global->impl_ << implement_struct_constructor(fields, short_name, scoped_name).c_str()					 
					 << implement_struct_properties(fields, scoped_name).c_str()
					 << implement_struct_to_native(fields, short_name, scoped_name).c_str()
					 << implement_struct_from_native(fields, short_name, scoped_name).c_str();


	IDL_GlobalData::DCPS_Data_Type_Info* info = idl_global->is_dcps_type(name);
	if (info) {
		std::string header = header_template_;
		replaceAll(header, replacements);
		be_global->header_ << header;

		std::string impl = impl_template_;
		replaceAll(impl, replacements);
		be_global->impl_ << impl;
	}*/

	return true;
}

bool cwrapper_generator::gen_union(AST_Union*, UTL_ScopedName* name, const std::vector<AST_UnionBranch*>&, AST_Type*, const char*)
{
	/*if (idl_global->is_dcps_type(name)) {
		std::cerr << "ERROR: union " << scoped(name) << " can not be used as a "
			"DCPS_DATA_TYPE (only structs can be Topic types)" << std::endl;
		return false;
	}

	//TODO: Implement unions for c++/cli
	std::cerr << "ERROR: union not implemented yet." << std::endl;*/

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
		
		ret.append(indent);
		ret.append(type_name.c_str());
		ret.append(" ");
		ret.append(name);
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

	//ret.append(scoped_name);
	//ret.append(" OpenDDSharp::");
	//ret.append(scoped_name);
	//ret.append("::ToNative() {\n");	

	///*ret.append("    ::");
	//ret.append(scoped_name);
	//ret.append("* ret = new ::");
	//ret.append(scoped_name);
	//ret.append("();\n");*/
	//ret.append("    ::");
	//ret.append(scoped_name);
	//ret.append(" ret;\n");

	//ret.append("    msclr::interop::marshal_context context;\n\n");
	//
	//for (unsigned int i = 0; i < fields.size(); i++) {
	//	AST_Field* field = fields[i];		
	//	AST_Type* field_type = field->field_type();
	//	const char * field_name = field->local_name()->get_string();		

	//	ret.append(get_field_to_native(field_type, field_name));
	//}

	//ret.append("\n    return ret;\n");

	//ret.append("}\n\n");

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

	/*ret.append(" OpenDDSharp::");
	ret.append(scoped_name);
	ret.append("::FromNative(::");
	ret.append(scoped_name);
	ret.append(" native) {\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();

		ret.append(get_field_from_native(field_type, field_name));
	}

	ret.append("}\n\n");*/

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
	std::string ret(type->flat_name());

	switch (node_type)
	{	
	case AST_Decl::NT_union:			
	case AST_Decl::NT_struct:		
	{
		/*ret = "OpenDDSharp::";
		ret.append(type->full_name());
		ret.append("^");*/
		break;
	}
	case AST_Decl::NT_enum:	
	{
		/*ret = "OpenDDSharp::";
		ret.append(type->full_name());*/
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
		//ret = "System::Decimal";
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
		/*AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_cwrapper_type(arr_type->base_type());
		
		ret = "array<";
		ret.append(base_type);
		ret.append(", ");
		ret.append(std::to_string(arr_type->n_dims()));
		ret.append(">^");*/
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

std::string cwrapper_generator::get_field_to_native(AST_Type* type, const char * name) {
	std::string ret("");
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		/*ret.append("    if (m_");
		ret.append(name);
		ret.append(" != nullptr) {\n");

		ret.append("        ret.");
		ret.append(name);
		ret.append(" = m_");
		ret.append(name);
		ret.append("->ToNative();\n");

		ret.append("    }\n");*/
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
	}
	case AST_Decl::NT_enum:
	{		
		/*ret.append("    ret.");
		ret.append(name);
		ret.append(" = (::");
		ret.append(scoped(type->name()));
		ret.append(")m_");
		ret.append(name);
		ret.append(";\n");*/
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
			ret.append("        const long double const_");
			ret.append(name);
			ret.append(" = ");
			ret.append(name);
			ret.append(";\n");
			ret.append("        ret.");
			ret.append(name);
			ret.append(".assign(const_");
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
			ret.append("Wrapper");
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

			ret.append("            ACE_UINT32 length = aux.length();");

			ret.append("            ret.");
			ret.append(name);
			ret.append(".length(length);\n");

			ret.append("            for (ACE_UINT32 i = 0; i < length; i++)\n");

			ret.append("            {");

			ret.append("                ret.");
			ret.append(name);
			ret.append("[i] = aux[i].to_native();\n");

			ret.append("            }");
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

				ret.append("                ret.");
				ret.append(name);
				ret.append("[i].assign(aux[i]);\n");
				
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
		/*ret.append("    m_");
		ret.append(name);
		ret.append("->FromNative(native.");
		ret.append(name);
		ret.append(");\n");*/
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
		/*ret.append("    m_");
		ret.append(name);
		ret.append(" = (::OpenDDSharp::");
		ret.append(scoped(type->name()));
		ret.append(")native.");
		ret.append(name);
		ret.append(";\n");*/
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
		ret.append("        ");
		ret.append(name);
		ret.append(" = native.");
		ret.append(name);
		ret.append(";\n");		
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
			/*{
				TAO::unbounded_value_sequence<NestedTestStructWrapper> aux;
				ACE_UINT32 length = nativeData.StructSequence.length();
				aux.length(length);
				for (ACE_UINT32 i = 0; i < length; i++)
				{
					aux[i].from_native(nativeData.StructSequence[i]);
				}
				marshal::unbounded_sequence_to_ptr(aux, StructSequence);
			}*/
			// TODO
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
		/*ret.append("    m_");
		ret.append(name);
		ret.append("->FromNative(native.");
		ret.append(name);
		ret.append(");\n");*/
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
	case AST_Decl::NT_enum:
	{
		/*ret.append("    m_");
		ret.append(name);
		ret.append(" = (::OpenDDSharp::");
		ret.append(scoped(type->name()));
		ret.append(")native.");
		ret.append(name);
		ret.append(";\n");*/
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_release(typedef_type->base_type(), name);
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		/*ret.append("        ");
		ret.append(name);
		ret.append(" = native.");
		ret.append(name);
		ret.append(";\n");*/
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
			ret.append("        marshal::release_structure_sequence_ptr<");
			ret.append(base_type);
			ret.append("Wrapper>(");
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
	}
	}

	return ret;
}

std::string cwrapper_generator::get_typedef_seq_to_native(AST_Typedef* typedef_type, std::string field_name) {
	std::string ret("");

	AST_Sequence * seq_type = AST_Sequence::narrow_from_decl(typedef_type->base_type());
	std::string typedef_full_name = typedef_type->full_name();

	ret.append("    if (m_");
	ret.append(field_name);
	ret.append(" != nullptr) {\n");

	unsigned int bound = seq_type->max_size()->ev()->u.ulval;
	if (bound > 0) {
		ret.append("        int seq_");
		ret.append(field_name);
		ret.append("_length = System::Math::Min((int)ret.");
		ret.append(field_name);
		ret.append(".maximum(), m_");
		ret.append(field_name);
		ret.append("->Count);\n");
	}
	else {
		ret.append("        int seq_");
		ret.append(field_name);
		ret.append("_length = m_");
		ret.append(field_name);
		ret.append("->Count;\n");
	}
	
	ret.append("        ret.");
	ret.append(field_name);
	ret.append(".length(seq_");
	ret.append(field_name);
	ret.append("_length);\n");

	ret.append("        for (int i = 0; i < seq_");
	ret.append(field_name);
	ret.append("_length; i++) {\n");
	
	switch (seq_type->base_type()->node_type())
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret.append("            if (m_" + field_name + "[i] != nullptr) {\n");

		ret.append("                ret.");
		ret.append(field_name);
		ret.append("[i] = ");
		ret.append("m_");
		ret.append(field_name);
		ret.append("[i]->ToNative();\n");

		ret.append("            }\n");

		ret.append("            else {\n");

		ret.append("                ::");
		ret.append(seq_type->base_type()->full_name());
		ret.append(" aux = {};\n");

		ret.append("                ret.");
		ret.append(field_name);
		ret.append("[i] = aux;\n");

		ret.append("            }\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{		
		ret.append("            if (m_" + field_name + "[i] != nullptr) {\n");

		ret.append("                ret.");
		ret.append(field_name);
		ret.append("[i] = ");
		if (seq_type->base_type()->node_type() == AST_Decl::NT_string) {
			ret.append("context.marshal_as<const char*>(m_");
		}
		else {
			ret.append("context.marshal_as<const wchar_t*>(m_");
		}
		ret.append(field_name);
		ret.append("[i]);\n");

		ret.append("            }\n");

		ret.append("            else {\n");

		ret.append("                ret.");
		ret.append(field_name);
		ret.append("[i] = \"\";\n");

		ret.append("            }\n");
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(seq_type->base_type());
		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {
			ret.append("            ret.");
			ret.append(field_name);
			ret.append("[i] = ");
			ret.append("m_");
			ret.append(field_name);
			ret.append("[i];\n");
		}
		else {
			ret.append("            const long double const_aux = m_");
			ret.append(field_name);
			ret.append("[i];\n");

			ret.append("            ret.");
			ret.append(field_name);
			ret.append("[i].assign(const_aux);\n");
		}
		break;
	}
	}	

	ret.append("        }\n");

	ret.append("    }\n");
	ret.append("    else {\n");

	ret.append("        ret.");
	ret.append(field_name);
	ret.append(".length(0);\n");

	ret.append("    }\n");

	return ret;
}

std::string cwrapper_generator::get_typedef_seq_from_native(AST_Typedef* typedef_type, std::string field_name) {
	std::string ret("");

	AST_Sequence * seq_type = AST_Sequence::narrow_from_decl(typedef_type->base_type());
	std::string typedef_full_name = typedef_type->full_name();

	ret.append("    m_");
	ret.append(field_name);
	ret.append(" = gcnew List<");
	ret.append(get_cwrapper_type(seq_type->base_type()));
	ret.append(">(");
	unsigned int bound = seq_type->max_size()->ev()->u.ulval;
	if (bound > 0) {
		ret.append(std::to_string(bound));
	}
	ret.append(");\n");

	ret.append("    ::");
	ret.append(typedef_full_name);
	ret.append(" seq_");
	ret.append(field_name);
	ret.append(" = native.");
	ret.append(field_name);
	ret.append(";\n");

	ret.append("    for (unsigned int i = 0; i < seq_");
	ret.append(field_name);
	ret.append(".length(); i++) {\n");

		
	switch (seq_type->base_type()->node_type()) 
	{
	case AST_Decl::NT_struct:
	case AST_Decl::NT_union:
		ret.append("        OpenDDSharp::");
		ret.append(seq_type->base_type()->full_name());
		ret.append("^ aux = gcnew OpenDDSharp::");
		ret.append(seq_type->base_type()->full_name());
		ret.append("();\n");
		ret.append("        aux->FromNative(seq_");
		ret.append(field_name);
		ret.append("[i]);\n");
		ret.append("        m_");
		ret.append(field_name);
		ret.append("->Add(aux);\n");
		break;
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
		ret.append("        m_");
		ret.append(field_name);
		ret.append("->Add(");
		ret.append("gcnew System::String(");
		ret.append("seq_");
		ret.append(field_name);
		ret.append("[i]));\n");
		break;
	default:		
		ret.append("        m_");
		ret.append(field_name);
		ret.append("->Add(");
		ret.append("seq_");
		ret.append(field_name);
		ret.append("[i]);\n");
		break;
	}

	ret.append("    }\n");

	return ret;
}

std::string cwrapper_generator::get_typedef_array_to_native(AST_Typedef* typedef_type, std::string field_name) {
	std::string ret("");

	AST_Array * arr_type = AST_Array::narrow_from_decl(typedef_type->base_type());
	unsigned int n_dims = arr_type->n_dims();
	AST_Expression** dims = arr_type->dims();

	ret.append("    if (m_" + field_name + " != nullptr) {\n");

	for (unsigned int i = 0; i < n_dims; i++) {				
		ret.append("        unsigned int dim" + std::to_string(i) + "_" + field_name + " = ");
		ret.append("System::Math::Min(m_" + field_name + "->GetLength(" + std::to_string(i) + "), " + std::to_string(dims[i]->ev()->u.ulval) + ");\n");		
	}

	for (unsigned int i = 0; i < n_dims; i++) {
		std::string dim = "i_dim" + std::to_string(i);
		ret.append("        for (unsigned int " + dim + " = 0; " + dim + " < dim" + std::to_string(i) + "_" + field_name + "; " + dim + "++) {\n");
	}

	
	switch (arr_type->base_type()->node_type())
	{
	case AST_Decl::NT_struct:
	case AST_Decl::NT_union:
	{
		ret.append("            if (m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] != nullptr) {\n");

		ret.append("                ret." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(" = ");
		ret.append("m_" + field_name);
		ret.append("[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("]->ToNative();\n");

		ret.append("            }\n");
		ret.append("            else {\n");

		ret.append("                ::");
		ret.append(arr_type->base_type()->full_name());
		ret.append(" aux = {};\n");

		ret.append("                ret." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(" = aux;\n");

		ret.append("            }\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret.append("            if (m_" + field_name);
		ret.append("[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] != nullptr) {\n");

		ret.append("                ret." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(" = ");
		if (arr_type->base_type()->node_type() == AST_Decl::NT_string) {
			ret.append("context.marshal_as<const char*>(m_" + field_name);
		}
		else {
			ret.append("context.marshal_as<const wchar_t*>(m_" + field_name);
		}
		ret.append("[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("]);\n");

		ret.append("            }\n");

		ret.append("            else {\n");

		ret.append("                ret." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(" = \"\";\n");

		ret.append("            }\n");
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());
		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {
			ret.append("            ret." + field_name);
			for (unsigned int i = 0; i < n_dims; i++) {
				ret.append("[i_dim" + std::to_string(i) + "]");
			}
			ret.append(" = ");
			ret.append("m_" + field_name);
			ret.append("[");
			for (unsigned int i = 0; i < n_dims; i++) {
				ret.append("i_dim" + std::to_string(i));
				if (i + 1 < n_dims) {
					ret.append(", ");
				}
			}
			ret.append("];\n");
		}
		else {
			ret.append("            const long double const_aux = m_" + field_name + "[");			
			for (unsigned int i = 0; i < n_dims; i++) {
				ret.append("i_dim" + std::to_string(i));
				if (i + 1 < n_dims) {
					ret.append(", ");
				}
			}
			ret.append("];\n");

			ret.append("            ret." + field_name);
			for (unsigned int i = 0; i < n_dims; i++) {
				ret.append("[i_dim" + std::to_string(i) + "]");
			}
			ret.append(".assign(const_aux);\n");
		}
		break;
	}
	}
	
	for (unsigned int i = 0; i < n_dims; i++) {
		ret.append("        }\n");
	}
	ret.append("    }\n");
	return ret;
}

std::string cwrapper_generator::get_typedef_array_from_native(AST_Typedef* typedef_type, std::string field_name) {
	std::string ret("");

	AST_Array * arr_type = AST_Array::narrow_from_decl(typedef_type->base_type());
	unsigned int n_dims = arr_type->n_dims();
	AST_Expression** dims = arr_type->dims();

	ret.append("    m_" + field_name + " = gcnew array<" + get_cwrapper_type(arr_type->base_type()) + ", " + std::to_string(n_dims) + ">(");
	ret.append(std::to_string(dims[0]->ev()->u.ulval));
	unsigned int i = 1;
	while (i < n_dims) {
		ret.append(", ");
		ret.append(std::to_string(dims[i]->ev()->u.ulval));
		i++;
	}
	ret.append(");\n");

	for (unsigned int i = 0; i < n_dims; i++) {
		std::string dim = "i_dim" + std::to_string(i);
		ret.append("    for (unsigned int " + dim + " = 0; " + dim + " < " + std::to_string(dims[i]->ev()->u.ulval) + "; " + dim + "++) {\n");
	}

	switch (arr_type->base_type()->node_type())
	{
	case AST_Decl::NT_struct:
	case AST_Decl::NT_union:
	{
		ret.append("        m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] = gcnew OpenDDSharp::");
		ret.append(arr_type->base_type()->full_name());
		ret.append("();\n");

		ret.append("        m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("]->FromNative(native." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(");\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{		
		ret.append("        if (native." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(" != NULL) {\n");

		ret.append("            m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] = gcnew System::String(native." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(");\n");

		ret.append("        }\n");
		
		ret.append("        else {\n");

		ret.append("            m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] = \"\";\n");

		ret.append("        }\n");
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		ret.append("        m_" + field_name + "[");
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("i_dim" + std::to_string(i));
			if (i + 1 < n_dims) {
				ret.append(", ");
			}
		}
		ret.append("] = native." + field_name);
		for (unsigned int i = 0; i < n_dims; i++) {
			ret.append("[i_dim" + std::to_string(i) + "]");
		}
		ret.append(";\n");
		break;
	}
	}

	for (unsigned int i = 0; i < n_dims; i++) {
		ret.append("    }\n");
	}

	return ret;
}
