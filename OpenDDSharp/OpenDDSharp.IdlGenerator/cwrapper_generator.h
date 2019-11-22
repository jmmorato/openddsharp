/*
*
*
* Distributed under the OpenDDS License.
* See: http://www.opendds.org/license.html
*/

#ifndef cwrapper_generator_H
#define cwrapper_generator_H

#include "dds_generator.h"
#include "ast_field.h"
#include "ast_decl.h"
#include "utl_identifier.h"

#include <string>
#include <vector>

class cwrapper_generator : public dds_generator {
public:
	cwrapper_generator();

	bool gen_struct(AST_Structure* node, UTL_ScopedName* name, const std::vector<AST_Field*>& fields, AST_Type::SIZE_TYPE size, const char* repoid);

	bool gen_union(AST_Union*, UTL_ScopedName*, const std::vector<AST_UnionBranch*>&, AST_Type*, const char*);

	bool gen_module(AST_Module* node);

	bool gen_module_end();

	bool gen_enum(AST_Enum* node, UTL_ScopedName* name, const std::vector<AST_EnumVal*>& contents, const char* repoid);

	bool gen_const(UTL_ScopedName* name, bool nestedInInteface, AST_Constant* constant);

	bool gen_typedef(AST_Typedef* node, UTL_ScopedName* name, AST_Type* base, const char* repoid);

private:
	std::string header_template_;
	std::string impl_template_;

	std::string declare_struct_fields(const std::vector<AST_Field*>& fields);
	std::string implement_struct_to_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name);
	std::string implement_struct_from_native(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name);
	std::string implement_struct_release(const std::vector<AST_Field*>& fields, const std::string name, const std::string scoped_name);
	std::string get_cwrapper_type(AST_Type* type);	
	std::string get_field_to_native(AST_Type* type, const char * name);
	std::string get_field_from_native(AST_Type* type, const char * name);
	std::string get_field_release(AST_Type* type, const char * name);
	std::string get_typedef_seq_to_native(AST_Typedef* typedef_type, std::string field_name);
	std::string get_typedef_seq_from_native(AST_Typedef* typedef_type, std::string field_name);
	std::string get_typedef_array_to_native(AST_Typedef* typedef_type, std::string field_name);
	std::string get_typedef_array_from_native(AST_Typedef* typedef_type, std::string field_name);
};

#endif

