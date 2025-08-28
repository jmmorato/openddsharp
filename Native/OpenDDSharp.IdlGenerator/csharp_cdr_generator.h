/*
*
*
* Distributed under the OpenDDS License.
* See: http://www.opendds.org/license.html
*/

#ifndef csharp_cdr_generator_H
#define csharp_cdr_generator_H

#include "dds_generator.h"
#include "ast_field.h"
#include <unordered_set>
#include <string>
#include <vector>

class csharp_cdr_generator final : public dds_generator {

public:
    csharp_cdr_generator();

    bool gen_struct(AST_Structure *structure, UTL_ScopedName *name, const std::vector<AST_Field *> &fields,
                    AST_Type::SIZE_TYPE size, const char *repoid);

    bool gen_union(AST_Union *, UTL_ScopedName *, const std::vector<AST_UnionBranch *> &, AST_Type *, const char *);

    bool gen_module(AST_Module *node);

    bool gen_module_end();

    bool gen_enum(AST_Enum *node, UTL_ScopedName *name, const std::vector<AST_EnumVal *> &contents, const char *repoid);

    bool gen_const(UTL_ScopedName *name, bool nestedInInteface, AST_Constant *constant);

    bool gen_typedef(AST_Typedef *node, UTL_ScopedName *name, AST_Type *base, const char *repoid);

private:
  // C# Reserved Keywords (from official Microsoft docs)
  const std::unordered_set<std::string> csharpReservedWords = {
    // Reserved keywords
    "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char",
    "checked", "class", "const", "continue", "decimal", "default", "delegate",
    "do", "double", "else", "enum", "event", "explicit", "extern", "false",
    "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit",
    "in", "int", "interface", "internal", "is", "lock", "long", "namespace",
    "new", "null", "object", "operator", "out", "override", "params", "private",
    "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
    "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
    "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
    "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",

    // Contextual keywords (commonly included for completeness)
    "add", "alias", "ascending", "async", "await", "by", "descending",
    "dynamic", "equals", "from", "get", "global", "group", "into", "join",
    "let", "nameof", "on", "orderby", "partial", "remove", "select", "set",
    "value", "var", "when", "where", "yield"
  };

  std::string impl_template_;

  bool isCSharpReserved(const std::string& s);

  std::string declare_struct_fields(const std::vector<AST_Field *> &fields, const std::string indent);

  std::string declare_marshal_fields(const std::vector<AST_Field *> &fields, const std::string indent);

  std::string implement_struct_constructor(const std::vector<AST_Field *> &fields, const std::string name,
                                             const std::string indent);

  std::string implement_struct_properties(const std::vector<AST_Field *> &fields, const std::string indent);

  std::string implement_struct_memberwise_copy(const std::vector<AST_Field *> &fields, const std::string name,
                                                 const std::string indent);

  std::string get_csharp_type(AST_Type *type);

  std::string get_csharp_default_value(AST_Type *type, const char *name);

  std::string get_csharp_constructor_initialization(AST_Type *type, const char *name);

  std::string get_csharp_struct_array_constructor_initialization(AST_Type *type, const char *name, std::string loop_indent);

  std::string implement_to_cdr(const std::vector<AST_Field *> &fields, const std::string indent);

  std::string implement_to_cdr_field(AST_Type *field_type, std::string field_name, std::string indent);

  std::string implement_from_cdr(const std::vector<AST_Field *> &fields, const std::string indent);

  std::string implement_from_cdr_field(AST_Type *field_type, std::string field_name, std::string indent);

  std::string read_cdr_multi_array(std::string name, std::string csharp_base_type, std::string read_method, AST_Expression **dims, int total_dim, std::string indent);

  std::string read_cdr_enum_multi_array(std::string name, std::string csharp_base_type, std::string read_method, AST_Expression **dims, int total_dim, std::string indent);

  std::string read_cdr_struct_multi_array(std::string name, std::string csharp_base_type, AST_Expression **dims, int total_dim, std::string indent);

  std::string write_cdr_multi_array(std::string name, std::string csharp_base_type, std::string write_method, AST_Expression **dims, int total_dim, std::string indent);

  std::string write_cdr_enum_multi_array(std::string name, std::string csharp_base_type, std::string write_method, AST_Expression **dims, int total_dim, std::string indent);

  std::string write_cdr_struct_multi_array(std::string name, std::string csharp_base_type, AST_Expression **dims, int total_dim, std::string indent);
};

#endif

