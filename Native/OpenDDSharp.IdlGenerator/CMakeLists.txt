project(openddsharp_idl)
cmake_minimum_required(VERSION 3.8.2)

find_package(OpenDDS REQUIRED)

add_executable(openddsharp_idl
all.h
annotations.h annotations.cpp
be_extern.h
be_global.h be_global.cpp
be_init.cpp
be_produce.cpp
be_util.h be_util.cpp
cli_generator.h cli_generator.cpp
csharp_generator.h csharp_generator.cpp
cwrapper_generator.h cwrapper_generator.cpp
dds_generator.h dds_generator.cpp
dds_visitor.h dds_visitor.cpp
drv_args.cpp
drv_preproc.cpp
marshal.h
openddsharp_idl.cpp
topic_keys.h topic_keys.cpp)

string(REPLACE "\\" "/" TAO_ROOT_INCLUDE $ENV{TAO_ROOT}/TAO_IDL/include)
message("ACE_ROOT=${ACE_ROOT}" )
message("TAO_ROOT_INCLUDE=${TAO_ROOT_INCLUDE}" )

target_include_directories(openddsharp_idl PRIVATE ${TAO_ROOT_INCLUDE})
target_link_libraries(openddsharp_idl OpenDDS::Dcps TAO::IDL_FE)
