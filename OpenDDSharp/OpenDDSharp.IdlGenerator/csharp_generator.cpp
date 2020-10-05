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
#include "csharp_generator.h"
#include "be_extern.h"
#include "be_util.h"

#include "utl_identifier.h"

#include "ace/OS_NS_sys_stat.h"

#include <cstring>
#include <fstream>
#include <sstream>
#include <map>
#include <iostream>

namespace {
	std::string read_template(const char* prefix) {
		std::string path = be_util::dds_root();
		path.append("/dds/idl/");
		path.append(prefix);
		path.append("Template.txt");
		std::ifstream ifs(path.c_str());
		std::ostringstream oss;
		oss << ifs.rdbuf();
		return oss.str();
	}

	void replaceAll(std::string& s, const std::map<std::string, std::string>& rep) {
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

csharp_generator::csharp_generator()
	: impl_template_(read_template("CSharpImpl"))
{
}

bool csharp_generator::gen_module(AST_Module* node) {
	be_global->impl_ << "namespace " << node->name()->last_component()->get_string() << "\n{\n";

	return true;
}

bool csharp_generator::gen_module_end() {
	be_global->impl_ << "}\n";

	return true;
}

bool csharp_generator::gen_const(UTL_ScopedName* name, bool nestedInInteface, AST_Constant* constant) {
	std::string csharp_type("");
	std::string str_value("");

	switch (constant->et()) {
	case AST_Expression::EV_short:
		str_value = std::to_string(constant->constant_value()->ev()->u.sval);
		csharp_type = "System.Int16";
		break;
	case AST_Expression::EV_ushort:
		str_value = std::to_string(constant->constant_value()->ev()->u.usval);
		csharp_type = "System.UInt16";
		break;
	case AST_Expression::EV_long:
		str_value = std::to_string(constant->constant_value()->ev()->u.lval);
		csharp_type = "System.Int32";
		break;
	case AST_Expression::EV_ulong:
		str_value = std::to_string(constant->constant_value()->ev()->u.ulval);
		csharp_type = "System.UInt32";
		break;
	case AST_Expression::EV_float:
		str_value = std::to_string(constant->constant_value()->ev()->u.fval);
		str_value.append("f");
		csharp_type = "System.Single";
		break;
	case AST_Expression::EV_double:
		str_value = std::to_string(constant->constant_value()->ev()->u.dval);
		csharp_type = "System.Double";
		break;
	case AST_Expression::EV_char:
	case AST_Expression::EV_wchar:
	{
		std::ostringstream value("");
		constant->constant_value()->dump(value);
		str_value = "'";
		str_value.append(value.str());
		str_value.append("'");
		csharp_type = "System.Char";
		break;
	}
	case AST_Expression::EV_octet:
		str_value = std::to_string(constant->constant_value()->ev()->u.oval);
		csharp_type = "System.Byte";
		break;
	case AST_Expression::EV_bool:
		str_value = constant->constant_value()->ev()->u.bval ? "true" : "false";
		csharp_type = "System.Boolean";		
		break;
	case AST_Expression::EV_string:
		str_value = "\"";
		str_value.append(constant->constant_value()->ev()->u.strval->get_string());
		str_value.append("\"");
		csharp_type = "System.String";
		break;
	case AST_Expression::EV_wstring:
		str_value = "\"";
		str_value.append(constant->constant_value()->ev()->u.wstrval);
		str_value.append("\"");
		csharp_type = "System.String";
		break;
	case AST_Expression::EV_longlong:
		str_value = std::to_string(constant->constant_value()->ev()->u.llval);
		csharp_type = "System.Int64";
		break;
	case AST_Expression::EV_ulonglong:
		str_value = std::to_string(constant->constant_value()->ev()->u.ullval);
		csharp_type = "System.UInt64";
		break;
	case AST_Expression::EV_fixed:
		str_value = std::to_string(constant->constant_value()->ev()->u.fixedval);
		csharp_type = "System.Decimal";
		break;
	case AST_Expression::EV_enum:
	{
		std::ostringstream value("");
		constant->constant_value()->n()->dump(value);
		str_value = replaceString(std::string(value.str()), std::string("::"), std::string("."));

		std::ostringstream type("");
		constant->enum_full_name()->dump(type);
		std::string str_type = type.str();
		if (str_type.rfind("::", 0) == 0) {
			str_type = str_type.substr(2);			
		}
		csharp_type = replaceString(std::string(str_type), std::string("::"), std::string("."));
		break;
	}
	default:
		// CODE REVIEW: Error and stop?
		return true;
	}

	be_global->impl_ << "    public static class " << name->last_component()->get_string() << "\n"
					 << "    {\n"
					 << "        public static readonly " << csharp_type << " Value = " << str_value << ";\n"
				     << "    }\n\n";	

	return true;
}

bool csharp_generator::gen_enum(AST_Enum* node, UTL_ScopedName* name, const std::vector<AST_EnumVal*>& contents, const char* repoid) {
	be_global->impl_ << "    #region " << name->last_component()->get_string() << " Enumeration\n"
					 << "    public enum " << name->last_component()->get_string() << "\n"
					 << "    {\n";

	for (unsigned int i = 0; i < contents.size(); i++) {
		AST_EnumVal* val = contents[i];
	
		be_global->impl_ << "        " << val->local_name()->get_string() << " = ";
		val->constant_value()->dump(be_global->impl_);

		if (i + 1 < contents.size()) {
			be_global->impl_ << ",";
		}

		be_global->impl_ << "\n";
	}

	be_global->impl_ << "    }\n";
	be_global->impl_ << "    #endregion\n\n";

	return true;
}

bool csharp_generator::gen_typedef(AST_Typedef* node, UTL_ScopedName* name, AST_Type* base, const char* repoid) {
	return true;
}

bool csharp_generator::gen_struct(AST_Structure* structure, UTL_ScopedName* name, const std::vector<AST_Field*>& fields, AST_Type::SIZE_TYPE, const char*)
{		
	const std::string scoped_name = scoped(name);
	const std::string short_name = name->last_component()->get_string();

	std::map<std::string, std::string> replacements;
	replacements["SCOPED"] = scoped_name;
	replacements["TYPE"] = short_name;
	replacements["SEQ"] = be_global->sequence_suffix().c_str();
	replacements["SCOPED_METHOD"] = replaceString(std::string(scoped_name), std::string("."), std::string("_"));

	be_global->impl_ << "    #region " << short_name << " Definitions\n"
					 << "    public class " << short_name << "\n"
					 << "    {\n"
					 << "        #region Constants\n"
					 << "#if DEBUG\n"
					 << "        internal const string API_DLL_X86 = \"" << be_global->project_name() << "Wrapperd_Win32\";\n"
					 << "        internal const string API_DLL_X64 = \"" << be_global->project_name() << "Wrapperd_x64\";\n"
				     << "#else\n"
					 << "        internal const string API_DLL_X86 = \"" << be_global->project_name() << "Wrapper_Win32\";\n"
					 << "        internal const string API_DLL_X64 = \"" << be_global->project_name() << "Wrapper_x64\";\n"
					 << "#endif\n"
					 << "        #endregion\n\n"
					 << "        #region Fields" << "\n"
					 << declare_struct_fields(fields, "        ").c_str()
					 << "        #endregion" << "\n\n"
					 << "        #region Properties" << "\n"
					 << implement_struct_properties(fields, "        ").c_str()
					 << "        #endregion" << " \n\n"
					 << "        #region Constructors" << "\n"
					 << implement_struct_constructor(fields, short_name, "        ").c_str()
					 << "        #endregion" << "\n\n"
					 << "        #region Methods" << "\n"
					 << implement_struct_to_native(fields, short_name, "        ").c_str()
					 << implement_struct_from_native(fields, short_name, "        ").c_str()
					 << "        #endregion" << "\n"
					 << "    }\n\n";

	be_global->impl_ << "    [StructLayout(LayoutKind.Sequential)]\n"
					 << "    internal struct " << short_name << "Wrapper\n"
					 << "    {\n"					 
					 << declare_marshal_fields(fields, "        ").c_str()
					 << "    }\n\n";

	
	if (be_global->is_topic_type(structure)) {
		std::string impl = impl_template_;
		replaceAll(impl, replacements);
		be_global->impl_ << impl;
	}

	be_global->impl_ << "\n    #endregion" << "\n\n";

	return true;
}

bool csharp_generator::gen_union(AST_Union*, UTL_ScopedName* name, const std::vector<AST_UnionBranch*>&, AST_Type*, const char*)
{
	if (idl_global->is_dcps_type(name)) {
		std::cerr << "ERROR: union " << scoped(name) << " can not be used as a "
			"DCPS_DATA_TYPE (only structs can be Topic types)" << std::endl;
		return false;
	}

	//TODO: Implement unions.
	std::cerr << "ERROR: union not implemented yet." << std::endl;

	return true;
}

std::string csharp_generator::declare_struct_fields(const std::vector<AST_Field*>& fields, const std::string indent) {
	std::string ret("");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* type = field->field_type();		
		const char* name = field->local_name()->get_string();		
		std::string type_name = get_csharp_type(type);
		
		ret.append(indent);
		ret.append(type_name.c_str());
		ret.append(" _");
		ret.append(name);
		ret.append(";\n");		
	}

	return ret;
}

std::string csharp_generator::declare_marshal_fields(const std::vector<AST_Field*>& fields, const std::string indent) {
	std::string ret("");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* type = field->field_type();
		const char* name = field->local_name()->get_string();
		std::string marshalas = get_marshal_as_attribute(type);
		if (!marshalas.empty()) {
			ret.append(indent + marshalas);
		}
		std::string type_name = get_marshal_type(type);

		ret.append(indent);
		ret.append("public ");
		ret.append(type_name.c_str());
		ret.append(" ");
		ret.append(name);
		ret.append(";\n");
	}

	return ret;
}

std::string csharp_generator::implement_struct_constructor(const std::vector<AST_Field*>& fields, const std::string name, const std::string indent) {
	std::string ret(indent);
	ret.append("public ");
	ret.append(name);
	ret.append("()\n");
	ret.append(indent);
	ret.append("{\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* type = field->field_type();

		if (type->node_type() != AST_Decl::NT_enum) {
			char* field_name = field->local_name()->get_string();
			//field_name[0] = tolower(field_name[0]);
			std::string default_value = get_csharp_default_value(type);
			std::string initialization = get_csharp_constructor_initialization(type, field_name);

			ret.append(indent + "    _");
			ret.append(field_name);
			ret.append(" = ");
			ret.append(default_value);
			ret.append(";\n");
			ret.append(initialization);
		}
	}

	ret.append(indent + "}\n");
	return ret;
}

std::string csharp_generator::implement_struct_properties(const std::vector<AST_Field*>& fields, const std::string indent) {
	std::string ret("");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* type = field->field_type();
		const char* field_name = field->local_name()->get_string();
		std::string csharp_type = get_csharp_type(type);

		if (i != 0) {
			ret.append("\n");
		}
		ret.append(indent + "public ");
		ret.append(csharp_type);
		ret.append(" ");
		ret.append(field_name);
		ret.append("\n");
		ret.append(indent + "{\n");
		ret.append(indent + "    get { return _");
		ret.append(field_name);
		ret.append("; }\n");
		ret.append(indent + "    set { _");
		ret.append(field_name);
		ret.append(" = value; }\n");
		ret.append(indent + "}\n");
	}

	return ret;
}

std::string csharp_generator::implement_struct_to_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string indent) {
	std::string ret(indent);
	ret.append("internal ");
	ret.append(name);
	ret.append("Wrapper ToNative(List<IntPtr> toRelease)\n");
	ret.append(indent);
	ret.append("{\n");
	ret.append(indent);
	ret.append("    ");
	ret.append(name);
	ret.append("Wrapper wrapper = new ");
	ret.append(name);
	ret.append("Wrapper();\n\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];		
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();		

		ret.append(get_field_to_native(field_type, field_name, indent));
	}

	ret.append("\n");
	ret.append(indent);
	ret.append("    ");
	ret.append("return wrapper;\n");
	ret.append(indent);
	ret.append("}\n\n");

	return ret;
}

std::string csharp_generator::implement_struct_from_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string indent) {
	std::string ret(indent);
	ret.append("internal void FromNative(");
	ret.append(name);
	ret.append("Wrapper wrapper)\n");
	ret.append(indent);
	ret.append("{\n");

	for (unsigned int i = 0; i < fields.size(); i++) {
		AST_Field* field = fields[i];
		AST_Type* field_type = field->field_type();
		const char * field_name = field->local_name()->get_string();

		ret.append(get_field_from_native(field_type, field_name, indent));
	}

	ret.append(indent);
	ret.append("}\n");

	return ret;
}

std::string csharp_generator::get_csharp_type(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();	
	std::string ret(type->flat_name());

	switch (node_type)
	{	
	case AST_Decl::NT_union:			
	case AST_Decl::NT_struct:		
	{
		ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
		break;
	}
	case AST_Decl::NT_enum:	
	{
		ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_csharp_type(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		ret = "decimal";
		break;
	}
	case AST_Decl::NT_string:		
	case AST_Decl::NT_wstring:
	{
		ret = "string";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		switch (predefined_type->pt())
		{
		case AST_PredefinedType::PT_short:
			ret = "Int16";
			break;
		case AST_PredefinedType::PT_long:
			ret = "Int32";
			break;
		case AST_PredefinedType::PT_longlong:
			ret = "Int64";
			break;
		case AST_PredefinedType::PT_ushort:
			ret = "UInt16";
			break;
		case AST_PredefinedType::PT_ulong:
			ret = "UInt32";
			break;
		case AST_PredefinedType::PT_ulonglong:
			ret = "UInt64";
			break;
		case AST_PredefinedType::PT_float:
			ret = "Single";
			break;
		case AST_PredefinedType::PT_double:
			ret = "Double";
			break;
		case AST_PredefinedType::PT_longdouble:
			ret = "Decimal";
			break;
		case AST_PredefinedType::PT_octet:
			ret = "Byte";			
			break;
		case AST_PredefinedType::PT_char:
		case AST_PredefinedType::PT_wchar:
			ret = "Char";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "Boolean";
			break;
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_csharp_type(arr_type->base_type());
		
		ret = base_type;
		ret.append("[");
		for (unsigned int i = 1; i < arr_type->n_dims(); i++) {
			ret.append(",");
		}
		ret.append("]");
		break;
	}		
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_csharp_type(seq_type->base_type());

		ret = "IList<";
		ret.append(base_type);
		ret.append(">");
		break;
	}		
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_marshal_type(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();
	std::string ret(type->flat_name());

	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
		ret.append("Wrapper");
		break;
	}
	case AST_Decl::NT_enum:
	{
		ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_marshal_type(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		//ret = "System.Decimal";
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret = "IntPtr";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		switch (predefined_type->pt())
		{
		case AST_PredefinedType::PT_short:
			ret = "Int16";
			break;
		case AST_PredefinedType::PT_long:
			ret = "Int32";
			break;
		case AST_PredefinedType::PT_longlong:
			ret = "Int64";
			break;
		case AST_PredefinedType::PT_ushort:
			ret = "UInt16";
			break;
		case AST_PredefinedType::PT_ulong:
			ret = "UInt32";
			break;
		case AST_PredefinedType::PT_ulonglong:
			ret = "UInt64";
			break;
		case AST_PredefinedType::PT_float:
			ret = "Single";
			break;
		case AST_PredefinedType::PT_double:
			ret = "Double";
			break;
		case AST_PredefinedType::PT_longdouble:
			ret = "Double";
			break;
		case AST_PredefinedType::PT_octet:
			ret = "Byte";
			break;
		case AST_PredefinedType::PT_char:
		case AST_PredefinedType::PT_wchar:
			ret = "Char";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "Boolean";
			break;
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_marshal_type(arr_type->base_type());

		if (arr_type->n_dims() > 1) {
			ret = "IntPtr";
		} 
		else {
			ret = base_type;
			ret.append("[]");			
		}
		break;
	}
	case AST_Decl::NT_sequence:
	{
		ret = "IntPtr";		
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_marshal_as_attribute(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();
	std::string ret;

	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret.append("[MarshalAs(UnmanagedType.Struct)]\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_marshal_as_attribute(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		//ret = "System.Decimal";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		switch (predefined_type->pt())
		{		
		case AST_PredefinedType::PT_octet:
			ret = "[MarshalAs(UnmanagedType.U1)]\n";
			break;
		case AST_PredefinedType::PT_char:
			ret = "[MarshalAs(UnmanagedType.I1)]\n";
			break;
		case AST_PredefinedType::PT_wchar:
			ret = "[MarshalAs(UnmanagedType.I2)]\n";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "[MarshalAs(UnmanagedType.I1)]\n";
			break;
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);		

		if (arr_type->n_dims() == 1) {
			AST_Expression** dims = arr_type->dims();
			std::string unmanagedType = get_marshal_attribute_unmanaged_type(arr_type->base_type());

			ret = "[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.";
			ret.append(unmanagedType);
			ret.append(", SizeConst = ");
			ret.append(std::to_string(dims[0]->ev()->u.ulval));
			ret.append(")]\n");
		}
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_marshal_attribute_unmanaged_type(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();
	std::string ret("");

	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret = "Struct";
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_marshal_attribute_unmanaged_type(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		break;
	}
	case AST_Decl::NT_enum:
	{
		ret = "I4";
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret = "SysInt";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		switch (predefined_type->pt())
		{
		case AST_PredefinedType::PT_short:
			ret = "I2";
			break;
		case AST_PredefinedType::PT_long:
			ret = "I4";
			break;
		case AST_PredefinedType::PT_longlong:
			ret = "I8";
			break;
		case AST_PredefinedType::PT_ushort:
			ret = "U2";
			break;
		case AST_PredefinedType::PT_ulong:
			ret = "U4";
			break;
		case AST_PredefinedType::PT_ulonglong:
			ret = "U8";
			break;
		case AST_PredefinedType::PT_float:
			ret = "R4";
			break;
		case AST_PredefinedType::PT_double:
			ret = "R8";
			break;
		case AST_PredefinedType::PT_longdouble:
			ret = "R8";
			break;
		case AST_PredefinedType::PT_octet:
			ret = "U1";
			break;
		case AST_PredefinedType::PT_char:
			ret = "I1";
			break;
		case AST_PredefinedType::PT_wchar:
			ret = "I2";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "I1";
			break;
		}
		break;
	}
	}

	return ret;
}

std::string csharp_generator::get_csharp_default_value(AST_Type* type) {
	AST_Decl::NodeType node_type = type->node_type();	
	std::string ret(type->flat_name());

	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret = "new ";
		ret.append(replaceString(std::string(type->full_name()), std::string("::"), std::string(".")));
		ret.append("()");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_csharp_default_value(typedef_type->base_type());
		break;
	}
	case AST_Decl::NT_fixed:
	{
		ret = "0";
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{
		ret = "string.Empty";
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);
		switch (predefined_type->pt())
		{
		case AST_PredefinedType::PT_short:
		case AST_PredefinedType::PT_long:
		case AST_PredefinedType::PT_longlong:
		case AST_PredefinedType::PT_ushort:
		case AST_PredefinedType::PT_ulong:
		case AST_PredefinedType::PT_ulonglong:
		case AST_PredefinedType::PT_float:
		case AST_PredefinedType::PT_double:
		case AST_PredefinedType::PT_longdouble:
		case AST_PredefinedType::PT_octet:
			ret = "0";
			break;
		case AST_PredefinedType::PT_char:
		case AST_PredefinedType::PT_wchar:
			ret = "default(char)";
			break;
		case AST_PredefinedType::PT_boolean:
			ret = "false";
			break;
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_csharp_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			unsigned int total_dim = arr_type->n_dims();
			ret = "new ";
			ret.append(replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
			ret.append("[");
			ret.append(std::to_string(dims[0]->ev()->u.ulval));
			for (unsigned int i = 1; i < total_dim; i++) {
				ret.append(",");
				ret.append(std::to_string(dims[i]->ev()->u.ulval));
			}
			ret.append("]");
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			unsigned int total_dim = arr_type->n_dims();
			ret = "new string[";
			ret.append(std::to_string(dims[0]->ev()->u.ulval));
			for (unsigned int i = 1; i < total_dim; i++) {
				ret.append(",");
				ret.append(std::to_string(dims[i]->ev()->u.ulval));
			}
			ret.append("]");
			break;
		}
		default:
		{
			ret = "new ";
			ret.append(base_type);
			ret.append("[");
			ret.append(std::to_string(dims[0]->ev()->u.ulval));
			for (unsigned int i = 1; i < arr_type->n_dims(); i++) {
				ret.append(", ");
				ret.append(std::to_string(dims[i]->ev()->u.ulval));
			}
			ret.append("]");
			break;
		}
		}
		break;
	}
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_csharp_type(seq_type->base_type());
		
		ret = "new List<";
		ret.append(base_type);
		ret.append(">(");
		unsigned int bound = seq_type->max_size()->ev()->u.ulval;
		if (bound > 0) {
			ret.append(std::to_string(bound));
		}
		ret.append(")");
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_csharp_constructor_initialization(AST_Type* type, const char * name) {
	AST_Decl::NodeType node_type = type->node_type();
	std::string ret("");

	switch (node_type)
	{
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_csharp_constructor_initialization(typedef_type->base_type(), name);
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_csharp_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			std::string loop_indent("            ");
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				ret.append(loop_indent);
				ret.append("for (int i");
				ret.append(std::to_string(i));
				ret.append(" = 0; i");
				ret.append(std::to_string(i));
				ret.append(" < ");
				ret.append(std::to_string(dims[i]->ev()->u.ulval));
				ret.append("; ++i");
				ret.append(std::to_string(i));
				ret.append(") {\n");

				loop_indent.append("    ");
			}

			ret.append(loop_indent);
			ret.append(name);
			ret.append("[");
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				ret.append("i");
				ret.append(std::to_string(i));
				if (i + 1 < arr_type->n_dims()) {
					ret.append(", ");
				}
			}
			ret.append("] = new ");
			ret.append(replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
			ret.append("();\n");

			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				loop_indent.erase(0, 4);
				ret.append(loop_indent);
				ret.append("}\n");
			}
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			std::string loop_indent("            ");
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				ret.append(loop_indent);
				ret.append("for (int i");
				ret.append(std::to_string(i));
				ret.append(" = 0; i");
				ret.append(std::to_string(i));
				ret.append(" < ");
				ret.append(std::to_string(dims[i]->ev()->u.ulval));
				ret.append("; ++i");
				ret.append(std::to_string(i));
				ret.append(") {\n");

				loop_indent.append("    ");
			}

			ret.append(loop_indent);
			ret.append(name);
			ret.append("[");
			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				ret.append("i");
				ret.append(std::to_string(i));
				if (i + 1 < arr_type->n_dims()) {
					ret.append(", ");
				}
			}
			ret.append("] = string.Empty;\n");

			for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
				loop_indent.erase(0, 4);
				ret.append(loop_indent);
				ret.append("}\n");
			}
			break;
		}
		default:
		{
			break;
		}
		}
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_field_to_native(AST_Type* type, const char * name, const std::string indent) {
	std::string ret("");
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{
		ret.append(indent);
		ret.append("    if (");
		ret.append(name);
		ret.append(" != null)\n");

		ret.append(indent);
		ret.append("    {\n");

		ret.append(indent);
		ret.append("        wrapper.");
		ret.append(name);
		ret.append(" = ");
		ret.append(name);
		ret.append(".ToNative(toRelease);\n");

		ret.append(indent);
		ret.append("    }\n");
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{		
		ret.append(indent);
		ret.append("    if (");
		ret.append(name);
		ret.append(" != null)\n");

		ret.append(indent);
		ret.append("    {\n");

		ret.append(indent);
		ret.append("        wrapper.");
		ret.append(name);
		if (node_type == AST_Decl::NT_string) {
			ret.append(" = Marshal.StringToHGlobalAnsi(");
		}
		else {
			ret.append(" = Marshal.StringToHGlobalUni(");
		}
		ret.append(name);
		ret.append(");\n");

		ret.append(indent);
		ret.append("        toRelease.Add(wrapper.");
		ret.append(name);
		ret.append(");\n");

		ret.append(indent);
		ret.append("    }\n");
		break;
	}
	case AST_Decl::NT_enum:
	{
		ret.append(indent);
		ret.append("    wrapper.");
		ret.append(name);
		ret.append(" = ");
		ret.append(name);
		ret.append(";\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_to_native(typedef_type->base_type(), name, indent);
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);

		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {
			ret.append(indent);
			ret.append("    wrapper.");
			ret.append(name);
			ret.append(" = _");
			ret.append(name);
			ret.append(";\n");
		}
		else {
			ret.append(indent);
			ret.append("    wrapper.");
			ret.append(name);
			ret.append(" = Convert.ToDouble(_");
			ret.append(name);
			ret.append(");\n");			
		}
		break;
	}
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_csharp_type(seq_type->base_type());
		AST_Decl::NodeType base_node_type = seq_type->base_type()->node_type();

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			ret.append(indent);
			ret.append("    if (");
			ret.append(name);
			ret.append(" != null)\n");

			ret.append(indent);
			ret.append("    {\n");

			ret.append(indent);
			ret.append("        List<");
			ret.append(base_type);
			ret.append("Wrapper> aux = new List<");
			ret.append(base_type);
			ret.append("Wrapper>();\n");

			ret.append(indent);
			ret.append("        foreach(");
			ret.append(base_type);
			ret.append(" s in ");
			ret.append(name);
			ret.append(")\n");

			ret.append(indent);
			ret.append("        {\n");

			ret.append(indent);
			ret.append("            aux.Add(s.ToNative(toRelease));\n");

			ret.append(indent);
			ret.append("        }\n");

			ret.append(indent);
			ret.append("        MarshalHelper.SequenceToPtr(aux, ref wrapper.");
			ret.append(name);
			ret.append(");\n");

			ret.append(indent);
			ret.append("        toRelease.Add(wrapper.");
			ret.append(name);
			ret.append(");\n");

			ret.append(indent);
			ret.append("    }\n");
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{
			ret.append(indent);
			ret.append("    toRelease.AddRange(MarshalHelper.StringSequenceToPtr(");
			ret.append(name);
			ret.append(", ref wrapper.");
			ret.append(name);
			if (base_node_type == AST_Decl::NT_string) {
				ret.append(", false));\n");
			}
			else {
				ret.append(", true));\n");
			}

			ret.append(indent);
			ret.append("    toRelease.Add(wrapper.");
			ret.append(name);
			ret.append(");\n");

			break;
		}
		case AST_Decl::NT_enum:
		{
			ret.append(indent);
			ret.append("    MarshalHelper.EnumSequenceToPtr(");
			ret.append(name);
			ret.append(", ref wrapper.");
			ret.append(name);
			ret.append(");\n");

			ret.append(indent);
			ret.append("    toRelease.Add(wrapper.");
			ret.append(name);
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_pre_defined:
		{
			AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(seq_type->base_type());

			if (predefined_type->pt() == AST_PredefinedType::PT_char) {
				ret.append(indent);
				ret.append("    if (");
				ret.append(name);
				ret.append(" != null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        IList<byte> aux = System.Text.Encoding.ASCII.GetBytes(");
				ret.append(name);
				ret.append(".ToArray()).ToList();\n");

				ret.append(indent);
				ret.append("        MarshalHelper.SequenceToPtr(aux, ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("        toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("    }\n");

				break;
			}
			else if (predefined_type->pt() == AST_PredefinedType::PT_boolean) {
				ret.append(indent);
				ret.append("    MarshalHelper.BooleanSequenceToPtr(");
				ret.append(name);
				ret.append(", ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("    toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			else if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
				ret.append(indent);
				ret.append("    MarshalHelper.LongDoubleSequenceToPtr(");
				ret.append(name);
				ret.append(", ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("    toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
		}
		default:
		{
			ret.append(indent);
			ret.append("    MarshalHelper.SequenceToPtr(");
			ret.append(name);
			ret.append(", ref wrapper.");
			ret.append(name);
			ret.append(");\n");

			ret.append(indent);
			ret.append("    toRelease.Add(wrapper.");
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
		std::string base_type = get_csharp_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		if (arr_type->n_dims() == 1)
		{
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append(indent);
				ret.append("    if (");
				ret.append(name);
				ret.append(" != null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        wrapper.");
				ret.append(name);
				ret.append(" = new ");
				ret.append(replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
				ret.append("Wrapper[");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("];\n");				

				ret.append(indent);
				ret.append("        for (int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            if (");
				ret.append(name);
				ret.append("[i] != null)\n");

				ret.append(indent);
				ret.append("            {\n");

				ret.append(indent);
				ret.append("                wrapper.");
				ret.append(name);
				ret.append("[i] = ");
				ret.append(name);
				ret.append("[i].ToNative(toRelease);\n");

				ret.append(indent);
				ret.append("            }\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("    }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append(indent);
				ret.append("    if (");
				ret.append(name);
				ret.append(" != null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        wrapper.");
				ret.append(name);
				ret.append(" = new IntPtr[");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("];\n");

				ret.append(indent);
				ret.append("        for (int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            if (");
				ret.append(name);
				ret.append("[i] != null)\n");

				ret.append(indent);
				ret.append("            {\n");

				ret.append(indent);
				ret.append("                wrapper.");
				ret.append(name);
				if (base_node_type = AST_Decl::NT_string) {
					ret.append("[i] = Marshal.StringToHGlobalAnsi(");
				}
				else {
					ret.append("[i] = Marshal.StringToHGlobalUni(");
				}
				ret.append(name);
				ret.append("[i]);\n");

				ret.append(indent);
				ret.append("                toRelease.Add(wrapper.");
				ret.append(name);
				ret.append("[i]);\n");

				ret.append(indent);
				ret.append("            }\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("    }\n");

				break;
			}
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
					ret.append(indent);
					ret.append("    wrapper.");
					ret.append(name);
					ret.append(" = Array.ConvertAll(");
					ret.append(name);
					ret.append(", e => Convert.ToDouble(e));\n");
					break;
				}
			}
			default:
			{
				ret.append(indent);
				ret.append("    wrapper.");
				ret.append(name);
				ret.append(" = ");
				ret.append(name);
				ret.append(";\n");
				break;
			}
			}
		}
		else {
			ret.append(indent);
			ret.append("    if (");
			ret.append(name);
			ret.append(" != null)\n");

			ret.append(indent);
			ret.append("    {\n");

			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ACE_UINT32 total_dim = 1;
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					total_dim *= dims[i]->ev()->u.ulval;
				}

				ret.append(indent);
				ret.append("        ");
				ret.append(base_type);
				ret.append("Wrapper[] aux = new ");
				ret.append(base_type);
				ret.append("Wrapper[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append(indent);
				ret.append("        int i = 0;\n");

				ret.append(indent);
				ret.append("        foreach(");
				ret.append(base_type);
				ret.append(" s in ");
				ret.append(name);
				ret.append(")\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            if (s != null)\n");

				ret.append(indent);
				ret.append("                aux[i] = s.ToNative(toRelease);\n\n");

				ret.append(indent);
				ret.append("            i++;\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("        MarshalHelper.MultiArrayToPtr<");
				ret.append(base_type);
				ret.append("Wrapper>(aux, ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("        toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append(indent);
				ret.append("    toRelease.AddRange(MarshalHelper.StringMultiArrayToPtr(");
				ret.append(name);
				ret.append(", ref wrapper.");
				ret.append(name);
				if (base_node_type == AST_Decl::NT_string) {
					ret.append(", false));\n");
				}
				else {
					ret.append(", true));\n");
				}

				ret.append(indent);
				ret.append("    toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_enum:
			{
				ret.append(indent);
				ret.append("        MarshalHelper.EnumMultiArrayToPtr<");
				ret.append(base_type);
				ret.append(">(");
				ret.append(name);
				ret.append(", ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("        toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_boolean) {
					
					ret.append(indent);
					ret.append("        MarshalHelper.BooleanMultiArrayToPtr(");
					ret.append(name);
					ret.append(", ref wrapper.");
					ret.append(name);
					ret.append(");\n");

					ret.append(indent);
					ret.append("        toRelease.Add(wrapper.");
					ret.append(name);
					ret.append(");\n");
					break;
				}
				else if (predefined_type->pt() == AST_PredefinedType::PT_char) {

					ret.append(indent);
					ret.append("        MarshalHelper.MultiArrayToPtr<byte>(");
					ret.append(name);
					ret.append(", ref wrapper.");
					ret.append(name);
					ret.append(");\n");

					ret.append(indent);
					ret.append("        toRelease.Add(wrapper.");
					ret.append(name);
					ret.append(");\n");
					break;
				}
				else if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
					ret.append(indent);
					ret.append("        double[");
					for (ACE_UINT32 i = 1; i < arr_type->n_dims(); i++) {
						ret.append(",");						
					}
					ret.append("] aux = new double[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}						
					}
					ret.append("];\n");
					
					std::string loop_indent(indent);
					loop_indent.append("        ");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(loop_indent);
						ret.append("for (int i");
						ret.append(std::to_string(i));
						ret.append(" = 0; i");
						ret.append(std::to_string(i));
						ret.append(" < ");
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						ret.append("; ++i");
						ret.append(std::to_string(i));
						ret.append(") {\n");

						loop_indent.append("    ");
					}

					ret.append(loop_indent);
					ret.append("aux[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("i");
						ret.append(std::to_string(i));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}
					}
					ret.append("]  = Convert.ToDouble(");
					ret.append(name);
					ret.append("[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("i");
						ret.append(std::to_string(i));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}
					}
					ret.append("]);\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						loop_indent.erase(0, 4);
						ret.append(loop_indent);
						ret.append("}\n");
					}

					ret.append(indent);
					ret.append("        MarshalHelper.MultiArrayToPtr<Double>(aux, ref wrapper.");
					ret.append(name);
					ret.append(");\n");

					ret.append(indent);
					ret.append("        toRelease.Add(wrapper.");
					ret.append(name);
					ret.append(");\n");
					break;
				}
			}
			default:
			{
				ret.append(indent);
				ret.append("        MarshalHelper.MultiArrayToPtr<");
				ret.append(base_type);
				ret.append(">(");
				ret.append(name);
				ret.append(", ref wrapper.");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("        toRelease.Add(wrapper.");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			}

			ret.append(indent);
			ret.append("    }\n");
		}
		break;
	}
	default:
		break;
	}

	return ret;
}

std::string csharp_generator::get_field_from_native(AST_Type* type, const char * name, const std::string indent) {
	std::string ret(indent);
	AST_Decl::NodeType node_type = type->node_type();
	switch (node_type)
	{
	case AST_Decl::NT_union:
	case AST_Decl::NT_struct:
	{		
		ret.append("    ");
		ret.append(name);
		ret.append(".FromNative(wrapper.");
		ret.append(name);
		ret.append(");\n");		
		break;
	}
	case AST_Decl::NT_string:
	case AST_Decl::NT_wstring:
	{		
		ret.append("    if (wrapper.");
		ret.append(name);
		ret.append(" != IntPtr.Zero)\n");

		ret.append(indent);
		ret.append("    {\n");

		ret.append(indent);
		ret.append("        ");
		ret.append(name);
		if (node_type == AST_Decl::NT_string) {
			ret.append("= Marshal.PtrToStringAnsi(wrapper.");
		}
		else {
			ret.append("= Marshal.PtrToStringUni(wrapper.");
		}
		ret.append(name);
		ret.append(");\n");

		ret.append(indent);
		ret.append("    }\n");

		ret.append(indent);
		ret.append("    else\n");

		ret.append(indent);
		ret.append("    {\n");

		ret.append(indent);
		ret.append("        ");
		ret.append(name);
		ret.append(" = null;\n");

		ret.append(indent);
		ret.append("    }\n");
		break;
	}
	case AST_Decl::NT_enum:
	{		
		ret.append("    ");
		ret.append(name);
		ret.append(" = ");
		ret.append("wrapper.");
		ret.append(name);
		ret.append(";\n");
		break;
	}
	case AST_Decl::NT_typedef:
	{
		AST_Typedef* typedef_type = AST_Typedef::narrow_from_decl(type);
		ret = get_field_from_native(typedef_type->base_type(), name, indent);
		break;
	}
	case AST_Decl::NT_pre_defined:
	{
		AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(type);
		if (predefined_type->pt() != AST_PredefinedType::PT_longdouble) {
			ret.append("    _");
			ret.append(name);
			ret.append(" = wrapper.");
			ret.append(name);
			ret.append(";\n");
		}
		else {
			ret.append("    _");
			ret.append(name);
			ret.append(" = Convert.ToDecimal(wrapper.");
			ret.append(name);
			ret.append(");\n");
		}
		break;
	}
	case AST_Decl::NT_sequence:
	{
		AST_Sequence* seq_type = AST_Sequence::narrow_from_decl(type);
		std::string base_type = get_csharp_type(seq_type->base_type());
		AST_Decl::NodeType base_node_type = seq_type->base_type()->node_type();
		unsigned int bound = seq_type->max_size()->ev()->u.ulval;

		switch (base_node_type)
		{
		case AST_Decl::NT_union:
		case AST_Decl::NT_struct:
		{
			ret.append("    if (wrapper.");
			ret.append(name);
			ret.append(" != IntPtr.Zero)\n");

			ret.append(indent);
			ret.append("    {\n");

			ret.append(indent);
			ret.append("        IList<");
			ret.append(base_type);
			ret.append("Wrapper> aux = new List<");
			ret.append(base_type);
			ret.append("Wrapper>();\n");

			ret.append(indent);
			ret.append("        MarshalHelper.PtrToSequence(wrapper.");
			ret.append(name);
			ret.append(", ref aux);\n");

			ret.append(indent);
			ret.append("        foreach(");
			ret.append(base_type);
			ret.append("Wrapper native in aux)\n");

			ret.append(indent);
			ret.append("        {\n");

			ret.append(indent);
			ret.append("            ");
			ret.append(base_type);
			ret.append(" s = new ");
			ret.append(base_type);
			ret.append("();\n");

			ret.append(indent);
			ret.append("            s.FromNative(native);\n");

			ret.append(indent);
			ret.append("            ");
			ret.append(name);
			ret.append(".Add(s);\n");

			ret.append(indent);
			ret.append("        }\n");

			ret.append(indent);
			ret.append("    }\n");
			break;
		}
		case AST_Decl::NT_string:
		case AST_Decl::NT_wstring:
		{			
			ret.append("    MarshalHelper.PtrToStringSequence(wrapper.");
			ret.append(name);
			ret.append(", ref _");
			ret.append(name);
			if (base_node_type == AST_Decl::NT_string) {
				ret.append(", false");
			}
			else {
				ret.append(", true");
			}
			if (bound > 0) {
				ret.append(", ");
				ret.append(std::to_string(bound));
			}
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_enum:
		{			
			ret.append("    MarshalHelper.PtrToEnumSequence(wrapper.");
			ret.append(name);
			ret.append(", ref _");
			ret.append(name);
			if (bound > 0) {
				ret.append(", ");
				ret.append(std::to_string(bound));
			}
			ret.append(");\n");
			break;
		}
		case AST_Decl::NT_pre_defined:
		{
			AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(seq_type->base_type());

			if (predefined_type->pt() == AST_PredefinedType::PT_boolean) {				
				ret.append("    MarshalHelper.PtrToBooleanSequence(wrapper.");
				ret.append(name);
				ret.append(", ref _");
				ret.append(name);
				if (bound > 0) {
					ret.append(", ");
					ret.append(std::to_string(bound));
				}
				ret.append(");\n");
				break;
			}
			else if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
				ret.append("    MarshalHelper.PtrToLongDoubleSequence(wrapper.");
				ret.append(name);
				ret.append(", ref _");
				ret.append(name);
				if (bound > 0) {
					ret.append(", ");
					ret.append(std::to_string(bound));
				}
				ret.append(");\n");
				break;
			}
		}
		default:
		{			
			ret.append("    MarshalHelper.PtrToSequence(wrapper.");
			ret.append(name);
			ret.append(", ref _");
			ret.append(name);
			if (bound > 0) {
				ret.append(", ");
				ret.append(std::to_string(bound));
			}
			ret.append(");\n");
			break;
		}
		}
		break;
	}
	case AST_Decl::NT_array:
	{
		AST_Array* arr_type = AST_Array::narrow_from_decl(type);
		std::string base_type = get_csharp_type(arr_type->base_type());
		AST_Expression** dims = arr_type->dims();
		AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

		if (arr_type->n_dims() == 1)
		{
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ret.append("    for (int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(name);
				ret.append("[i] = new ");
				ret.append(replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
				ret.append("();\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(name);
				ret.append("[i].FromNative(wrapper.");
				ret.append(name);
				ret.append("[i]);\n");

				ret.append(indent);
				ret.append("    }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{
				ret.append("    for (int i = 0; i < ");
				ret.append(std::to_string(dims[0]->ev()->u.ulval));
				ret.append("; i++)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        if (wrapper.");
				ret.append(name);
				ret.append("[i] != null)\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            ");
				ret.append(name);
				if (base_node_type = AST_Decl::NT_string) {
					ret.append("[i] = Marshal.PtrToStringAnsi(wrapper.");
				}
				else {
					ret.append("[i] = Marshal.PtrToStringUni(wrapper.");
				}
				ret.append(name);
				ret.append("[i]);\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("    }\n");
				break;
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
					ret.append("    ");
					ret.append(name);
					ret.append(" = Array.ConvertAll(wrapper.");
					ret.append(name);
					ret.append(", e => Convert.ToDecimal(e));\n");
					break;
				}
			}
			}
			default:
			{
				ret.append("    ");
				ret.append(name);
				ret.append(" = wrapper.");
				ret.append(name);
				ret.append(";\n");
				break;
			}
			}
		}
		else {
			switch (base_node_type)
			{
			case AST_Decl::NT_union:
			case AST_Decl::NT_struct:
			{
				ACE_UINT32 total_dim = 1;
				for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
					total_dim *= dims[i]->ev()->u.ulval;
				}

				ret.append("    if (wrapper.");
				ret.append(name);
				ret.append(" != null)\n");

				ret.append(indent);
				ret.append("    {\n");				

				ret.append(indent);
				ret.append("        if (");
				ret.append(name);
				ret.append(" == null)\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            ");
				ret.append(name);
				ret.append(" = ");
				ret.append(get_csharp_default_value(type));
				ret.append(";\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(base_type);
				ret.append("Wrapper[] aux_");
				ret.append(name);
				ret.append(" = new ");
				ret.append(base_type);
				ret.append("Wrapper[");
				ret.append(std::to_string(total_dim));
				ret.append("];\n");

				ret.append(indent);
				ret.append("        MarshalHelper.PtrToMultiArray<");
				ret.append(base_type);
				ret.append("Wrapper>(wrapper.");
				ret.append(name);
				ret.append(", aux_");
				ret.append(name);
				ret.append(");\n");

				ret.append(indent);
				ret.append("        int[] dimensions = new int[");
				ret.append(name);
				ret.append(".Rank];\n");

				ret.append(indent);
				ret.append("        for (int i = 0; i < ");
				ret.append(std::to_string(total_dim));
				ret.append("; i++)\n");

				ret.append(indent);
				ret.append("        {\n");

				ret.append(indent);
				ret.append("            if (i > 0)\n");

				ret.append(indent);
				ret.append("                MarshalHelper.UpdateDimensionsArray(");
				ret.append(name);
				ret.append(", dimensions);\n\n");

				ret.append(indent);
				ret.append("            ");
				ret.append(base_type);
				ret.append(" aux = new ");
				ret.append(base_type);
				ret.append("();\n");

				ret.append(indent);
				ret.append("            aux.FromNative(aux_");
				ret.append(name);
				ret.append("[i]);\n");

				ret.append(indent);
				ret.append("            ");
				ret.append(name);
				ret.append(".SetValue(aux, dimensions);\n");

				ret.append(indent);
				ret.append("        }\n");

				ret.append(indent);
				ret.append("    }\n");
				break;
			}
			case AST_Decl::NT_string:
			case AST_Decl::NT_wstring:
			{				
				ret.append("    if (");
				ret.append(name);
				ret.append(" == null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(name);
				ret.append(" = ");
				ret.append(get_csharp_default_value(type));
				ret.append(";\n");

				ret.append(indent);
				ret.append("    }\n");

				ret.append(indent);
				ret.append("    MarshalHelper.PtrToStringMultiArray(wrapper.");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				if (base_node_type == AST_Decl::NT_string) {
					ret.append(", false);\n");
				}
				else {
					ret.append(", true);\n");
				}
				break;
			}
			case AST_Decl::NT_enum:
			{
				ret.append("    if (");
				ret.append(name);
				ret.append(" == null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(name);
				ret.append(" = ");
				ret.append(get_csharp_default_value(type));
				ret.append(";\n");

				ret.append(indent);
				ret.append("    }\n");

				ret.append(indent);
				ret.append("    MarshalHelper.PtrToEnumMultiArray<");
				ret.append(base_type);
				ret.append(">(wrapper.");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			case AST_Decl::NT_pre_defined:
			{
				AST_PredefinedType * predefined_type = AST_PredefinedType::narrow_from_decl(arr_type->base_type());

				if (predefined_type->pt() == AST_PredefinedType::PT_boolean) {					
					ret.append("    if (");
					ret.append(name);
					ret.append(" == null)\n");

					ret.append(indent);
					ret.append("    {\n");

					ret.append(indent);
					ret.append("        ");
					ret.append(name);
					ret.append(" = ");
					ret.append(get_csharp_default_value(type));
					ret.append(";\n");

					ret.append(indent);
					ret.append("    }\n");

					ret.append(indent);
					ret.append("    MarshalHelper.PtrToBooleanMultiArray(wrapper.");
					ret.append(name);
					ret.append(", ");
					ret.append(name);
					ret.append(");\n");
					break;
				}
				else if (predefined_type->pt() == AST_PredefinedType::PT_char) {
					ret.append("    if (");
					ret.append(name);
					ret.append(" == null)\n");

					ret.append(indent);
					ret.append("    {\n");

					ret.append(indent);
					ret.append("        ");
					ret.append(name);
					ret.append(" = ");
					ret.append(get_csharp_default_value(type));
					ret.append(";\n");

					ret.append(indent);
					ret.append("    }\n");

					ret.append(indent);
					ret.append("    MarshalHelper.PtrToMultiArray<byte>(wrapper.");					
					ret.append(name);
					ret.append(", ");
					ret.append(name);
					ret.append(");\n");
					break;
				}
				else if (predefined_type->pt() == AST_PredefinedType::PT_longdouble) {
					ret.append("    if (");
					ret.append(name);
					ret.append(" == null)\n");

					ret.append(indent);
					ret.append("    {\n");

					ret.append(indent);
					ret.append("        ");
					ret.append(name);
					ret.append(" = ");
					ret.append(get_csharp_default_value(type));
					ret.append(";\n");

					ret.append(indent);
					ret.append("    }\n");

					ret.append(indent);
					ret.append("    {\n");

					ret.append(indent);
					ret.append("        double[");
					for (ACE_UINT32 i = 1; i < arr_type->n_dims(); i++) {
						ret.append(",");
					}
					ret.append("] aux = new double[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}
					}
					ret.append("];\n");

					ret.append(indent);
					ret.append("        MarshalHelper.PtrToMultiArray<Double>(wrapper.");
					ret.append(name);
					ret.append(", aux);\n");

					std::string loop_indent(indent);
					loop_indent.append("        ");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append(loop_indent);
						ret.append("for (int i");
						ret.append(std::to_string(i));
						ret.append(" = 0; i");
						ret.append(std::to_string(i));
						ret.append(" < ");
						ret.append(std::to_string(dims[i]->ev()->u.ulval));
						ret.append("; ++i");
						ret.append(std::to_string(i));
						ret.append(") {\n");

						loop_indent.append("    ");
					}

					ret.append(loop_indent);
					ret.append(name);
					ret.append("[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("i");
						ret.append(std::to_string(i));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}
					}
					ret.append("] = Convert.ToDecimal(aux[");
					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						ret.append("i");
						ret.append(std::to_string(i));
						if (i + 1 < arr_type->n_dims()) {
							ret.append(", ");
						}
					}
					ret.append("]);\n");

					for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
						loop_indent.erase(0, 4);
						ret.append(loop_indent);
						ret.append("}\n");
					}

					ret.append(indent);
					ret.append("    }\n");
					break;
				}
			}
			default:
			{				
				ret.append("    if (");
				ret.append(name);
				ret.append(" == null)\n");

				ret.append(indent);
				ret.append("    {\n");

				ret.append(indent);
				ret.append("        ");
				ret.append(name);
				ret.append(" = ");
				ret.append(get_csharp_default_value(type));
				ret.append(";\n");

				ret.append(indent);
				ret.append("    }\n");

				ret.append(indent);
				ret.append("    MarshalHelper.PtrToMultiArray<");
				ret.append(base_type);
				ret.append(">(wrapper.");
				ret.append(name);
				ret.append(", ");
				ret.append(name);
				ret.append(");\n");
				break;
			}
			}
		}
		break;
	}
	default:
		break;
	}

	return ret;
}
