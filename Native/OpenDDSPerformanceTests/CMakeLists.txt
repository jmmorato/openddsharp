cmake_minimum_required(VERSION 3.19.2)

project(OpenDDSPerformanceTests CXX)

if (APPLE)
elseif (UNIX)
    set(CMAKE_SKIP_BUILD_RPATH FALSE)
    set(CMAKE_BUILD_WITH_INSTALL_RPATH TRUE)
    set(CMAKE_INSTALL_RPATH $ORIGIN)
endif ()

find_package(OpenDDS REQUIRED)

add_library(OpenDDSPerformanceTests SHARED
        performance_tests.h performance_tests.cpp
        latency_test.h latency_test.cpp
        throughput_test.h throughput_test.cpp
        utils.h utils.cpp)

if (MSVC)
    add_compile_definitions(_CRT_SECURE_NO_WARNINGS)
    add_compile_definitions(_WINSOCK_DEPRECATED_NO_WARNINGS)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /bigobj /wd4190")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /O2 /Oi /D NDEBUG /D NOMINMAX /MD")
elseif (APPLE)
    set_target_properties(OpenDDSPerformanceTests PROPERTIES LINK_FLAGS "-Wl,-rpath,@executable_path -Wl,-rpath,@loader_path")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wpointer-arith -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare -pthread")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -O3 -DNDEBUG")
elseif (UNIX)
    set_target_properties(OpenDDSPerformanceTests PROPERTIES LINK_FLAGS "-Wl,-rpath,$ORIGIN")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wpointer-arith -Wno-switch -Wno-return-type-c-linkage -Wno-tautological-pointer-compare -pthread")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -O3 -DNDEBUG")
endif ()

target_include_directories(OpenDDSPerformanceTests PRIVATE "../build_osx-arm64/OpenDDSTestData/opendds_generated")
target_link_libraries(OpenDDSPerformanceTests OpenDDSTestData)
