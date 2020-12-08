# Install script for directory: C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/KiWi-master/KiWi-master/examples

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/Program Files (x86)/KiWi")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/custom-render/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/label/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/frame-family/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/editbox/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/styleswitcher/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/scrollbox/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/drag/cmake_install.cmake")
  include("C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Kiwi/Build/examples/debug-gizmos/cmake_install.cmake")

endif()

