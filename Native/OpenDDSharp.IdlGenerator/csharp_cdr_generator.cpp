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
#include "csharp_cdr_generator.h"
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
    std::string read_template(const char *prefix) {
      std::string path = be_util::dds_root();
      path.append("/dds/idl/");
      path.append(prefix);
      path.append("Template.txt");
      std::ifstream ifs(path.c_str());
      std::ostringstream oss;
      oss << ifs.rdbuf();
      return oss.str();
    }

    void replaceAll(std::string &s, const std::map<std::string, std::string> &rep) {
      typedef std::map<std::string, std::string>::const_iterator mapiter_t;
      for (size_t i = s.find("<%"); i < s.length(); i = s.find("<%", i + 1)) {
        size_t n = s.find("%>", i) - i + 2;
        mapiter_t iter = rep.find(s.substr(i + 2, n - 4));
        if (iter != rep.end()) {
          s.replace(i, n, iter->second);
        }
      }
    }

    std::string replaceString(std::string str, const std::string &from, const std::string &to) {
      size_t start_pos = 0;
      while ((start_pos = str.find(from, start_pos)) != std::string::npos) {
        str.replace(start_pos, from.length(), to);
        start_pos += to.length(); // Handles case where 'to' is a substring of 'from'
      }
      return str;
    }
}

csharp_cdr_generator::csharp_cdr_generator()
    : impl_template_(read_template("CSharpCDRImpl")) {
}

bool csharp_cdr_generator::gen_module(AST_Module *node) {
  be_global->impl_ << "namespace " << node->name()->last_component()->get_string() << "\n{\n";

  return true;
}

bool csharp_cdr_generator::gen_module_end() {
  be_global->impl_ << "}\n";

  return true;
}

bool csharp_cdr_generator::gen_const(UTL_ScopedName *name, bool nestedInInteface, AST_Constant *constant) {
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
    case AST_Expression::EV_wchar: {
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
      str_value = std::to_string(static_cast<long double>(constant->constant_value()->ev()->u.fixedval));
      csharp_type = "System.Decimal";
      break;
    case AST_Expression::EV_enum: {
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

bool csharp_cdr_generator::gen_enum(AST_Enum *node, UTL_ScopedName *name, const std::vector<AST_EnumVal *> &contents,
                                     const char *repoid) {
  be_global->impl_ << "    #region " << name->last_component()->get_string() << " Enumeration\n"
                   << "    public enum " << name->last_component()->get_string() << "\n"
                   << "    {\n";

  for (unsigned int i = 0; i < contents.size(); i++) {
    AST_EnumVal *val = contents[i];

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

bool csharp_cdr_generator::gen_typedef(AST_Typedef *node, UTL_ScopedName *name, AST_Type *base, const char *repoid) {
  return true;
}

bool csharp_cdr_generator::gen_struct(AST_Structure *structure, UTL_ScopedName *name,
                                       const std::vector<AST_Field *> &fields, AST_Type::SIZE_TYPE, const char *) {
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
                   << "        internal const string API_DLL = \"" << be_global->project_name() << "Wrapper\";\n"
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
                   << implement_struct_memberwise_copy(fields, short_name, "        ").c_str()
                   << implement_to_cdr(fields, "        ").c_str()
                   << implement_from_cdr(fields, "        ").c_str()
                   << "        #endregion" << "\n"
                   << "    }\n\n";

  if (be_global->is_topic_type(structure)) {
    std::string impl = impl_template_;
    replaceAll(impl, replacements);
    be_global->impl_ << impl;
  }

  be_global->impl_ << "\n    #endregion" << "\n\n";

  return true;
}

bool
csharp_cdr_generator::gen_union(AST_Union *, UTL_ScopedName *name, const std::vector<AST_UnionBranch *> &, AST_Type *,
                                 const char *) {
  if (idl_global->is_dcps_type(name)) {
    std::cerr << "ERROR: union " << scoped(name) << " can not be used as a "
                                                    "DCPS_DATA_TYPE (only structs can be Topic types)" << std::endl;
    return false;
  }

  //TODO: Implement unions.
  std::cerr << "ERROR: union not implemented yet." << std::endl;

  return true;
}

std::string
csharp_cdr_generator::declare_struct_fields(const std::vector<AST_Field *> &fields, const std::string indent) {
  std::string ret("");

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *type = field->field_type();
    const char *name = field->local_name()->get_string();
    std::string type_name = get_csharp_type(type);

    ret.append(indent);
    ret.append(type_name.c_str());
    ret.append(" _");
    ret.append(name);
    ret.append(";\n");
  }

  return ret;
}

std::string
csharp_cdr_generator::implement_struct_constructor(const std::vector<AST_Field *> &fields, const std::string name,
                                                    const std::string indent) {
  std::string ret(indent);
  ret.append("public ");
  ret.append(name);
  ret.append("()\n");
  ret.append(indent);
  ret.append("{\n");

  for (unsigned int i = 0; i < fields.size(); ++i) {
    AST_Field *field = fields[i];
    AST_Type *type = field->field_type();

    if (type->node_type() != AST_Decl::NT_enum) {
      char *field_name = field->local_name()->get_string();
      std::string default_value = get_csharp_default_value(type, field_name);
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

std::string
csharp_cdr_generator::implement_struct_properties(const std::vector<AST_Field *> &fields, const std::string indent) {
  std::string ret("");

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *type = field->field_type();
    const char *field_name = field->local_name()->get_string();
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

std::string
csharp_cdr_generator::implement_struct_memberwise_copy(const std::vector<AST_Field *> &fields, const std::string name,
                                                       const std::string indent) {
  std::string ret(indent);
  ret.append("internal void MemberwiseCopy(");
  ret.append(name);
  ret.append(" source)\n");

  ret.append(indent);
  ret.append("{\n");

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *field_type = field->field_type();
    const char *field_name = field->local_name()->get_string();

    ret.append(indent);
    ret.append("    ");
    ret.append(field_name);
    ret.append(" = source.");
    ret.append(field_name);
    ret.append(";\n");
  }

  ret.append(indent);
  ret.append("}\n\n");

  return ret;
}

std::string
csharp_cdr_generator::get_csharp_type(AST_Type *type) {
  AST_Decl::NodeType node_type = type->node_type();
  std::string ret(type->flat_name());

  switch (node_type) {
    case AST_Decl::NT_union:
    case AST_Decl::NT_struct: {
      ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
      break;
    }
    case AST_Decl::NT_enum: {
      ret = replaceString(std::string(type->full_name()), std::string("::"), std::string("."));
      break;
    }
    case AST_Decl::NT_typedef: {
      AST_Typedef *typedef_type = dynamic_cast<AST_Typedef*>(type);
      ret = get_csharp_type(typedef_type->base_type());
      break;
    }
    case AST_Decl::NT_fixed: {
      ret = "decimal";
      break;
    }
    case AST_Decl::NT_string:
    case AST_Decl::NT_wstring: {
      ret = "string";
      break;
    }
    case AST_Decl::NT_pre_defined: {
      AST_PredefinedType *predefined_type = dynamic_cast<AST_PredefinedType*>(type);

      switch (predefined_type->pt()) {
        case AST_PredefinedType::PT_int8:
          ret = "sbyte";
          break;
        case AST_PredefinedType::PT_uint8:
          ret = "byte";
          break;
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
    case AST_Decl::NT_array: {
      AST_Array *arr_type = dynamic_cast<AST_Array*>(type);
      std::string base_type = get_csharp_type(arr_type->base_type());

      ret = base_type;
      ret.append("[");
      for (unsigned int i = 1; i < arr_type->n_dims(); i++) {
        ret.append("][");
      }
      ret.append("]");
      break;
    }
    case AST_Decl::NT_sequence: {
      AST_Sequence *seq_type = dynamic_cast<AST_Sequence*>(type);
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

std::string
csharp_cdr_generator::get_csharp_default_value(AST_Type *type, const char *field_name) {
  AST_Decl::NodeType node_type = type->node_type();
  std::string ret(type->flat_name());

  switch (node_type) {
    case AST_Decl::NT_union:
    case AST_Decl::NT_struct: {
      ret = "new ";
      ret.append(replaceString(std::string(type->full_name()), std::string("::"), std::string(".")));
      ret.append("()");
      break;
    }
    case AST_Decl::NT_typedef: {
      AST_Typedef *typedef_type = dynamic_cast<AST_Typedef*>(type);
      ret = get_csharp_default_value(typedef_type->base_type(), field_name);
      break;
    }
    case AST_Decl::NT_fixed: {
      ret = "0";
      break;
    }
    case AST_Decl::NT_string:
    case AST_Decl::NT_wstring: {
      ret = "string.Empty";
      break;
    }
    case AST_Decl::NT_pre_defined: {
      AST_PredefinedType *predefined_type = dynamic_cast<AST_PredefinedType*>(type);
      switch (predefined_type->pt()) {
        case AST_PredefinedType::PT_int8:
        case AST_PredefinedType::PT_uint8:
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
    case AST_Decl::NT_array: {
      AST_Array *arr_type = dynamic_cast<AST_Array*>(type);
      std::string base_type = get_csharp_type(arr_type->base_type());
      AST_Expression **dims = arr_type->dims();
      AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();

      switch (base_node_type) {
        case AST_Decl::NT_union:
        case AST_Decl::NT_struct: {
          unsigned int total_dim = arr_type->n_dims();
          ret = "new ";
          ret.append(
              replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
          ret.append("[");
          ret.append(std::to_string(dims[0]->ev()->u.ulval));
          for (unsigned int i = 1; i < total_dim; i++) {
            ret.append("][");
          }
          ret.append("]");
          break;
        }
        case AST_Decl::NT_string:
        case AST_Decl::NT_wstring: {
          unsigned int total_dim = arr_type->n_dims();
          ret = "new string[";
          ret.append(std::to_string(dims[0]->ev()->u.ulval));
          for (unsigned int i = 1; i < total_dim; i++) {
            ret.append("][");
          }
          ret.append("]");
          break;
        }
        default: {
          unsigned int total_dim = arr_type->n_dims();
          std::string csharp_base_type = get_csharp_type(arr_type->base_type());

          // First dimension initialization
          ret = "new ";
          ret.append(csharp_base_type);
          ret.append("[");
          ret.append(std::to_string(dims[0]->ev()->u.ulval));
          for (unsigned int i = 1; i < total_dim; i++) {
            ret.append("][");
          }
          ret.append("]");
          break;
        }
      }
      break;
    }
    case AST_Decl::NT_sequence: {
      AST_Sequence *seq_type = dynamic_cast<AST_Sequence*>(type);
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

std::string
csharp_cdr_generator::get_csharp_constructor_initialization(AST_Type *type, const char *name) {
  AST_Decl::NodeType node_type = type->node_type();
  std::string ret("");

  switch (node_type) {
    case AST_Decl::NT_typedef: {
      AST_Typedef *typedef_type = dynamic_cast<AST_Typedef*>(type);
      ret = get_csharp_constructor_initialization(typedef_type->base_type(), name);
      break;
    }
    case AST_Decl::NT_array: {
      AST_Array *arr_type = dynamic_cast<AST_Array*>(type);
      std::string base_type = get_csharp_type(arr_type->base_type());
      AST_Expression **dims = arr_type->dims();
      AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();
      unsigned int total_dim = arr_type->n_dims();
      std::string csharp_base_type = get_csharp_type(arr_type->base_type());

      switch (base_node_type) {
        case AST_Decl::NT_union:
        case AST_Decl::NT_struct: {
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

            if (i + 1 < total_dim) {
              ret.append(loop_indent);
              ret.append(name);
              for (unsigned int j = 0; j < i + 1; ++j) {

                ret.append("[i");
                ret.append(std::to_string(j));
                ret.append("]");
              }
              ret.append(" = new ");
              ret.append(csharp_base_type);
              ret.append("[");
              ret.append(std::to_string(dims[i + 1]->ev()->u.ulval));
              ret.append("]");
              for (unsigned int j = i + 2; j < total_dim; ++j) {
                ret.append("[]");
              }
              ret.append(";\n");
            }
          }

          ret.append(loop_indent);
          ret.append(name);
          ret.append("[");
          for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
            ret.append("i");
            ret.append(std::to_string(i));
            if (i + 1 < arr_type->n_dims()) {
              ret.append("][");
            }
          }
          ret.append("] = new ");
          ret.append(
              replaceString(std::string(arr_type->base_type()->full_name()), std::string("::"), std::string(".")));
          ret.append("();\n");

          for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
            loop_indent.erase(0, 4);
            ret.append(loop_indent);
            ret.append("}\n");
          }
          break;
        }
        case AST_Decl::NT_string:
        case AST_Decl::NT_wstring: {
          std::string loop_indent("            ");
          for (ACE_UINT32 i = 0; i < total_dim; i++) {
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

            if (i + 1 < total_dim) {
              ret.append(loop_indent);
              ret.append(name);
              for (unsigned int j = 0; j < i + 1; ++j) {

                ret.append("[i");
                ret.append(std::to_string(j));
                ret.append("]");
              }
              ret.append(" = new ");
              ret.append(csharp_base_type);
              ret.append("[");
              ret.append(std::to_string(dims[i + 1]->ev()->u.ulval));
              ret.append("]");
              for (unsigned int j = i + 2; j < total_dim; ++j) {
                ret.append("[]");
              }
              ret.append(";\n");
            }
          }

          ret.append(loop_indent);
          ret.append(name);
          ret.append("[");
          for (ACE_UINT32 i = 0; i < arr_type->n_dims(); i++) {
            ret.append("i");
            ret.append(std::to_string(i));
            if (i + 1 < arr_type->n_dims()) {
              ret.append("][");
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
        default: {
          // Remaining dimensions initialization for jagger arrays
          std::string loop_indent("            ");
          for (unsigned int i = 1; i < total_dim; ++i) {
            ret.append(loop_indent + "for (int i");
            ret.append(std::to_string(i - 1));
            ret.append(" = 0; i");
            ret.append(std::to_string(i - 1));
            ret.append(" < ");
            ret.append(std::to_string(dims[i - 1]->ev()->u.ulval));
            ret.append(" ; ++i");
            ret.append(std::to_string(i - 1));
            ret.append(" ) {\n");

            loop_indent.append("    ");
            ret.append(loop_indent);
            ret.append(name);
            for (unsigned int j = 0; j < i; ++j) {

              ret.append("[i");
              ret.append(std::to_string(j));
              ret.append("]");
            }
            ret.append(" = new ");
            ret.append(csharp_base_type);
            ret.append("[");
            ret.append(std::to_string(dims[i]->ev()->u.ulval));
            ret.append("]");
            for (unsigned int j = i + 1; j < total_dim; ++j) {
              ret.append("[]");
            }
            ret.append(";\n");
          }

          for (unsigned int i = total_dim; i > 1; --i) {
            loop_indent.erase(0, 4);
            ret.append(loop_indent);
            ret.append("}\n");
          }
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

std::string
csharp_cdr_generator::implement_to_cdr(const std::vector<AST_Field *> &fields, const std::string indent)
{
  std::string ret(indent);
  ret.append("public ReadOnlySpan<byte> ToCDR()\n");
  ret.append(indent);
  ret.append("{\n");

  ret.append(indent);
  ret.append("    var writer = new OpenDDSharp.Marshaller.Cdr.CdrWriter();\n");

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *field_type = field->field_type();
    const char *field_name = field->local_name()->get_string();

    ret.append(implement_to_cdr_field(field_type, field_name, indent));
  }

  ret.append(indent);
  ret.append("    return writer.GetBuffer();\n");
  ret.append(indent);
  ret.append("}\n\n");

  return ret;
}

std::string
csharp_cdr_generator::implement_to_cdr_field(AST_Type *field_type, std::string field_name, std::string indent)
{
  std::string ret(indent);

  AST_Decl::NodeType node_type = field_type->node_type();
  switch (node_type) {
    case AST_Decl::NT_typedef: {
      AST_Typedef *typedef_type = dynamic_cast<AST_Typedef*>(field_type);
      ret = implement_to_cdr_field(typedef_type->base_type(), field_name, indent);
      break;
    }
    case AST_Decl::NT_string:
    case AST_Decl::NT_wstring: {
      ret.append("    writer.WriteString(");
      ret.append(field_name);
      ret.append(");\n");
      break;
    }
    case AST_Decl::NT_pre_defined: {
      AST_PredefinedType *predefined_type = dynamic_cast<AST_PredefinedType *>(field_type);
      switch (predefined_type->pt()) {
        case AST_PredefinedType::PT_int8:
        case AST_PredefinedType::PT_uint8:
          ret.append("    writer.WriteByte(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_short:
          ret.append("    writer.WriteInt16(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_long:
          ret.append("    writer.WriteInt32(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_longlong:
          ret.append("    writer.WriteInt64(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_ushort:
          ret.append("    writer.WriteUInt16(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_ulong:
          ret.append("    writer.WriteUInt32(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_ulonglong:
          ret.append("    writer.WriteUInt64(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_float:
          ret.append("    writer.WriteSingle(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_double:
          ret.append("    writer.WriteDouble(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_longdouble:
          ret.append("    writer.WriteDouble(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_octet:
          ret.append("    writer.WriteByte(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_char:
          ret.append("    writer.WriteChar(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_wchar:
          ret.append("    writer.WriteWChar(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        case AST_PredefinedType::PT_boolean:
          ret.append("    writer.WriteBool(");
          ret.append(field_name);
          ret.append(");\n");
          break;
        default:
          ret.append("    // ");
          ret.append(field_name);
          ret.append(": Not implemented yet.\n");
          break;
      }
      break;
    }
    case AST_Decl::NT_sequence: {
      AST_Sequence *seq_type = dynamic_cast<AST_Sequence*>(field_type);
      AST_Type *base_type = seq_type->base_type();
      AST_Decl::NodeType base_node_type = base_type->node_type();

      switch (base_node_type) {
        case AST_Decl::NT_pre_defined: {
          AST_PredefinedType *base_predefined_type = dynamic_cast<AST_PredefinedType *>(base_type);
          switch (base_predefined_type->pt()) {
            case AST_PredefinedType::PT_int8:
            case AST_PredefinedType::PT_uint8:
              ret.append("    writer.WriteByteSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_short:
              ret.append("    writer.WriteInt16Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_long:
              ret.append("    writer.WriteInt32Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_longlong:
              ret.append("    writer.WriteInt64Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_ushort:
              ret.append("    writer.WriteUInt16Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_ulong:
              ret.append("    writer.WriteUInt32Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_ulonglong:
              ret.append("    writer.WriteUInt64Sequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_float:
              ret.append("    writer.WriteSingleSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_double:
              ret.append("    writer.WriteDoubleSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_longdouble:
              ret.append("    writer.WriteDoubleSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_octet:
              ret.append("    writer.WriteByteSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_char:
              ret.append("    writer.WriteCharSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_wchar:
              ret.append("    writer.WriteWCharSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            case AST_PredefinedType::PT_boolean:
              ret.append("    writer.WriteBoolSequence(");
              ret.append(field_name);
              ret.append(");\n");
              break;
            default:
              ret.append(field_name);
              ret.append(": Not implemented yet.\n");
              break;
          }
          break;
        }
        default:
          break;
      }
      break;
    }
    case AST_Decl::NT_array: {
      AST_Array *arr_type = dynamic_cast<AST_Array *>(field_type);
      AST_Type *base_type = arr_type->base_type();
      AST_Expression **dims = arr_type->dims();
      AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();
      unsigned int total_dim = arr_type->n_dims();
      switch (base_node_type) {
        case AST_Decl::NT_pre_defined: {
          AST_PredefinedType *base_predefined_type = dynamic_cast<AST_PredefinedType *>(base_type);
          switch (base_predefined_type->pt()) {
            case AST_PredefinedType::PT_int8:
            case AST_PredefinedType::PT_uint8:
              if (total_dim == 1) {
                ret.append("    writer.WriteByteArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "byte", "WriteByte", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_short:
              if (total_dim == 1) {
                ret.append("    writer.WriteInt16Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "short", "WriteInt16", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_long:
              if (total_dim == 1) {
                ret.append("    writer.WriteInt32Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "int", "WriteInt32", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_longlong:
              if (total_dim == 1) {
                ret.append("    writer.WriteInt64Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "long", "WriteInt64", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ushort:
              if (total_dim == 1) {
                ret.append("    writer.WriteUInt16Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "ushort", "WriteUInt16", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ulong:
              if (total_dim == 1) {
                ret.append("    writer.WriteUInt32Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "uint", "WriteUInt32", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ulonglong:
              if (total_dim == 1) {
                ret.append("    writer.WriteUInt64Array(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "ulong", "WriteUInt64", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_float:
              if (total_dim == 1) {
                ret.append("    writer.WriteSingleArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "float", "WriteSingle", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_double:
              if (total_dim == 1) {
                ret.append("    writer.WriteDoubleArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "double", "WriteDouble", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_longdouble:
              if (total_dim == 1) {
                ret.append("    writer.WriteDoubleArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "double", "WriteDouble", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_octet:
              if (total_dim == 1) {
                ret.append("    writer.WriteByteArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "byte", "WriteByte", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_char:
              if (total_dim == 1) {
                ret.append("    writer.WriteCharArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "char", "WriteChar", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_wchar:
              if (total_dim == 1) {
                ret.append("    writer.WriteWCharArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "char", "WriteWChar", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_boolean:
              if (total_dim == 1) {
                ret.append("    writer.WriteBoolArray(");
                ret.append(field_name);
                ret.append(", ");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.erase(0, 8);
                ret.append(write_cdr_multi_array(field_name, "bool", "WriteBool", dims, total_dim, indent));
              }
              break;
            default:
              ret.append(field_name);
              ret.append(": Not implemented yet.\n");
              break;
          }
          break;
        }
      }
      break;
    }
  }

  return ret;
}

std::string
csharp_cdr_generator::implement_from_cdr(const std::vector<AST_Field *> &fields, const std::string indent)
{
  std::string ret(indent);
  ret.append("public void FromCDR(ReadOnlySpan<byte> data)\n");
  ret.append(indent);
  ret.append("{\n");

  ret.append(indent);
  ret.append("    var reader = new OpenDDSharp.Marshaller.Cdr.CdrReader(data.ToArray());\n");

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *field_type = field->field_type();
    const char *field_name = field->local_name()->get_string();

    ret.append(implement_from_cdr_field(field_type, field_name, indent));
  }

  ret.append(indent);
  ret.append("}\n");

  return ret;
}

std::string
csharp_cdr_generator::implement_from_cdr_field(AST_Type *field_type, std::string field_name, std::string indent)
{
  std::string ret(indent);

  AST_Decl::NodeType node_type = field_type->node_type();
  switch (node_type) {
    case AST_Decl::NT_typedef: {
      AST_Typedef *typedef_type = dynamic_cast<AST_Typedef *>(field_type);
      ret = implement_from_cdr_field(typedef_type->base_type(), field_name, indent);
      break;
    }
    case AST_Decl::NT_string:
    case AST_Decl::NT_wstring: {
      ret.append("    ");
      ret.append(field_name);
      ret.append(" = reader.ReadString();\n");
      break;
    }
    case AST_Decl::NT_pre_defined: {
      AST_PredefinedType *predefined_type = dynamic_cast<AST_PredefinedType *>(field_type);
      switch (predefined_type->pt()) {
        case AST_PredefinedType::PT_int8:
        case AST_PredefinedType::PT_uint8:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadByte();\n");
          break;
        case AST_PredefinedType::PT_short:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadInt16();\n");
          break;
        case AST_PredefinedType::PT_long:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadInt32();\n");
          break;
        case AST_PredefinedType::PT_longlong:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadInt64();\n");
          break;
        case AST_PredefinedType::PT_ushort:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadUInt16();\n");
          break;
        case AST_PredefinedType::PT_ulong:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadUInt32();\n");
          break;
        case AST_PredefinedType::PT_ulonglong:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadUInt64();\n");
          break;
        case AST_PredefinedType::PT_float:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadSingle();\n");
          break;
        case AST_PredefinedType::PT_double:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadDouble();\n");
          break;
        case AST_PredefinedType::PT_longdouble:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadDouble();\n");
          break;
        case AST_PredefinedType::PT_octet:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadByte();\n");
          break;
        case AST_PredefinedType::PT_char:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadChar();\n");
          break;
        case AST_PredefinedType::PT_wchar:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadWChar();\n");
          break;
        case AST_PredefinedType::PT_boolean:
          ret.append("    ");
          ret.append(field_name);
          ret.append(" = reader.ReadBool();\n");
          break;
      }
      break;
    }
    case AST_Decl::NT_sequence: {
      AST_Sequence *seq_type = dynamic_cast<AST_Sequence *>(field_type);
      AST_Type *base_type = seq_type->base_type();
      AST_Decl::NodeType base_node_type = base_type->node_type();

      switch (base_node_type) {
        case AST_Decl::NT_pre_defined: {
          AST_PredefinedType *base_predefined_type = dynamic_cast<AST_PredefinedType *>(base_type);
          switch (base_predefined_type->pt()) {
            case AST_PredefinedType::PT_int8:
            case AST_PredefinedType::PT_uint8:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadByteSequence();\n");
              break;
            case AST_PredefinedType::PT_short:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadInt16Sequence();\n");
              break;
            case AST_PredefinedType::PT_long:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadInt32Sequence();\n");
              break;
            case AST_PredefinedType::PT_longlong:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadInt64Sequence();\n");
              break;
            case AST_PredefinedType::PT_ushort:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadUInt16Sequence();\n");
              break;
            case AST_PredefinedType::PT_ulong:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadUInt32Sequence();\n");
              break;
            case AST_PredefinedType::PT_ulonglong:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadUInt64Sequence();\n");
              break;
            case AST_PredefinedType::PT_float:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadSingleSequence();\n");
              break;
            case AST_PredefinedType::PT_double:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadDoubleSequence();\n");
              break;
            case AST_PredefinedType::PT_longdouble:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadDoubleSequence();\n");
              break;
            case AST_PredefinedType::PT_octet:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadByteSequence();\n");
              break;
            case AST_PredefinedType::PT_char:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadCharSequence();\n");
              break;
            case AST_PredefinedType::PT_wchar:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadWCharSequence();\n");
              break;
            case AST_PredefinedType::PT_boolean:
              ret.append("    ");
              ret.append(field_name);
              ret.append(" = reader.ReadBoolSequence();\n");
              break;
            default:
              ret.append(field_name);
              ret.append(": Not implemented yet.\n");
              break;
          }
        }
      }
      break;
    }
    case AST_Decl::NT_array: {
      AST_Array *arr_type = dynamic_cast<AST_Array *>(field_type);
      AST_Type *base_type = arr_type->base_type();
      AST_Expression **dims = arr_type->dims();
      AST_Decl::NodeType base_node_type = arr_type->base_type()->node_type();
      unsigned int total_dim = arr_type->n_dims();
      switch (base_node_type) {
        case AST_Decl::NT_pre_defined: {
          AST_PredefinedType *base_predefined_type = dynamic_cast<AST_PredefinedType *>(base_type);
          switch (base_predefined_type->pt()) {
            case AST_PredefinedType::PT_int8:
            case AST_PredefinedType::PT_uint8:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadByteArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "byte", "ReadByte", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_short:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadInt16Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "short", "ReadInt16", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_long:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadInt32Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "int", "ReadInt32", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_longlong:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadInt64Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "long", "ReadInt64", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ushort:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadUInt16Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "ushort", "ReadUInt16", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ulong:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadUInt32Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "uint", "ReadUInt32", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_ulonglong:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadUInt64Array(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "ulong", "ReadUInt64", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_float:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadSingleArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "float", "ReadSingle", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_double:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadDoubleArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "double", "ReadDouble", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_longdouble:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadDoubleArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                  ret.append(read_cdr_multi_array(field_name, "double", "ReadDouble", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_octet:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadByteArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                  ret.append(read_cdr_multi_array(field_name, "byte", "ReadByte", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_char:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadCharArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                ret.append(read_cdr_multi_array(field_name, "char", "ReadChar", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_wchar:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadWCharArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                  ret.append(read_cdr_multi_array(field_name, "char", "ReadWChar", dims, total_dim, indent));
              }
              break;
            case AST_PredefinedType::PT_boolean:
              if (total_dim == 1) {
                ret.append("    ");
                ret.append(field_name);
                ret.append(" = reader.ReadBoolArray(");
                ret.append(std::to_string(dims[0]->ev()->u.ulval));
                ret.append(");\n");
              } else {
                  ret.append(read_cdr_multi_array(field_name, "bool", "ReadBool", dims, total_dim, indent));
              }
              break;
            default:
              ret.append(field_name);
              ret.append(": Not implemented yet.\n");
              break;
          }
          break;
        }
      }
    }
  }

  return ret;
}

std::string
csharp_cdr_generator::read_cdr_multi_array(std::string name, std::string csharp_base_type, std::string read_method, AST_Expression **dims, int total_dim, std::string indent)
{
  std::string ret("    ");

  ret.append(name);
  ret.append(" = new ");
  ret.append(csharp_base_type);
  ret.append("[");
  ret.append(std::to_string(dims[0]->ev()->u.ulval));
  ret.append("]");
  for (unsigned int i = 1; i < total_dim; ++i) {
    ret.append("[]");
  }
  ret.append(";\n");

  indent.append("    ");
  std::string loop_indent(indent);
  for (ACE_UINT32 i = 0; i < total_dim; i++) {
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

    if (i + 1 < total_dim) {
      ret.append(loop_indent);
      ret.append(name);
      for (unsigned int j = 0; j < i + 1; ++j) {

        ret.append("[i");
        ret.append(std::to_string(j));
        ret.append("]");
      }
      ret.append(" = new ");
      ret.append(csharp_base_type);
      ret.append("[");
      ret.append(std::to_string(dims[i + 1]->ev()->u.ulval));
      ret.append("]");
      for (unsigned int j = i + 2; j < total_dim; ++j) {
        ret.append("[]");
      }
      ret.append(";\n");
    }
  }

  ret.append(loop_indent);
  ret.append(name);
  ret.append("[");
  for (ACE_UINT32 i = 0; i < total_dim; i++) {
    ret.append("i");
    ret.append(std::to_string(i));
    if (i + 1 < total_dim) {
      ret.append("][");
    }
  }
  ret.append("] = reader.");
  ret.append(read_method);
  ret.append("();\n");

  for (ACE_UINT32 i = 0; i < total_dim; i++) {
    loop_indent.erase(0, 4);
    ret.append(loop_indent);
    ret.append("}\n");
  }

  return ret;
}

std::string
csharp_cdr_generator::write_cdr_multi_array(std::string name, std::string csharp_base_type, std::string write_method, AST_Expression **dims, int total_dim, std::string indent)
{
  std::string ret("");

  indent.append("    ");
  std::string loop_indent(indent);
  for (ACE_UINT32 i = 0; i < total_dim; i++) {
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
  ret.append("writer.");
  ret.append(write_method);
  ret.append("(");
  ret.append(name);
  ret.append("[");
  for (ACE_UINT32 i = 0; i < total_dim; i++) {
    ret.append("i");
    ret.append(std::to_string(i));
    if (i + 1 < total_dim) {
      ret.append("][");
    }
  }
  ret.append("]);\n");

  for (ACE_UINT32 i = 0; i < total_dim; i++) {
    loop_indent.erase(0, 4);
    ret.append(loop_indent);
    ret.append("}\n");
  }

  return ret;
}