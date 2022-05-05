project(openddsharp_idl CXX)
cmake_minimum_required(VERSION 3.8.2)

find_package(OpenDDS REQUIRED)

add_executable(openddsharp_idl
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

string(REPLACE "\\" "/" TAO_ROOT_INCLUDE ${TAO_ROOT}/TAO_IDL/include)
string(REPLACE "\\" "/" TAO_ROOT_BE_INCLUDE ${TAO_ROOT}/TAO_IDL/be_include)
message("ACE_ROOT=${ACE_ROOT}" )
message("TAO_ROOT=${TAO_ROOT}" )
message("DDS_ROOT=${DDS_ROOT}" )

if(MSVC)
  add_definitions(-D_CRT_SECURE_NO_WARNINGS)
  add_definitions(-D_WINSOCK_DEPRECATED_NO_WARNINGS)
endif()

if(APPLE)
  set_target_properties(openddsharp_idl PROPERTIES LINK_FLAGS "-Wl,-rpath,@executable_path/../../ACE_MacOS/lib -Wl,-rpath,@executable_path/../lib")
  target_compile_options(openddsharp_idl PUBLIC -Wno-switch)
endif()

target_include_directories(openddsharp_idl PRIVATE ${TAO_ROOT_BE_INCLUDE} PRIVATE ${TAO_ROOT_INCLUDE} PRIVATE ${TAO_ROOT})
target_link_libraries(openddsharp_idl OpenDDS::Dcps TAO::TAO TAO::IDL_FE)
