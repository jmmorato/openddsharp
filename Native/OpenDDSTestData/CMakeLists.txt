cmake_minimum_required(VERSION 3.19.2)

project(OpenDDSTestData CXX)

if (APPLE)
elseif (UNIX)
    set(CMAKE_SKIP_BUILD_RPATH FALSE)
    set(CMAKE_BUILD_WITH_INSTALL_RPATH TRUE)
    set(CMAKE_INSTALL_RPATH $ORIGIN)
endif ()

find_package(OpenDDS REQUIRED)

add_library(OpenDDSTestData SHARED)

if (MSVC)
    add_compile_definitions(_CRT_SECURE_NO_WARNINGS)
    add_compile_definitions(_WINSOCK_DEPRECATED_NO_WARNINGS)
    # MSVC 19.40+ (VS 2022 17.10+) removed stdext::checked_array_iterator used by ACE.
    # Define _STLPORT_VERSION to use ACE's fallback implementation (plain pointer return).
    if(CMAKE_CXX_COMPILER_VERSION VERSION_GREATER_EQUAL "19.40")
        add_compile_definitions(_STLPORT_VERSION=0)
    endif()
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /bigobj /wd4190 /MD")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /O2 /D NDEBUG /D NOMINMAX")
elseif (APPLE)
    set_target_properties(OpenDDSTestData PROPERTIES LINK_FLAGS "-Wl,-rpath,@executable_path -Wl,-rpath,@loader_path")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wpointer-arith -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare -pthread")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -O3 -DNDEBUG")
elseif (UNIX)
    set_target_properties(OpenDDSTestData PROPERTIES LINK_FLAGS "-Wl,-rpath,$ORIGIN")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wpointer-arith -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare -pthread")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -O3 -DNDEBUG")
endif ()

OPENDDS_TARGET_SOURCES(OpenDDSTestData
        PUBLIC
            TestData.idl
            OPENDDS_IDL_OPTIONS -Sa --no-dcps-data-type-warnings)

target_link_libraries(OpenDDSTestData OpenDDS::OpenDDS)
