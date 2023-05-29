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

  be_global->header_ << "void " << scoped_method << "_CopyKeys("
                     << scoped_name
                     << " *source, "
                     << scoped_name
                     << " *destination) {\n";

  for (unsigned int i = 0; i < fields.size(); i++) {
    AST_Field *field = fields[i];
    AST_Type *type = field->field_type();
    const char *name = field->local_name()->get_string();
    bool is_key = true;
    if (be_global->check_key(field, is_key)) {
      be_global->header_ << "  destination->" << name << " = source->" << name << ";\n";
    }
  }

  be_global->header_ << "};\n\n";

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
