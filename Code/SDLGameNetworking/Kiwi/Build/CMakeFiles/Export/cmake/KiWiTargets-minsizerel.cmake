#----------------------------------------------------------------
# Generated CMake target import file for configuration "MinSizeRel".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "KiWi" for configuration "MinSizeRel"
set_property(TARGET KiWi APPEND PROPERTY IMPORTED_CONFIGURATIONS MINSIZEREL)
set_target_properties(KiWi PROPERTIES
  IMPORTED_IMPLIB_MINSIZEREL "${_IMPORT_PREFIX}/lib/KiWi.lib"
  IMPORTED_LINK_INTERFACE_LIBRARIES_MINSIZEREL "C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Libraries/SDL2-2.0.9/lib/x86/SDL2.lib;C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Libraries/SDL2_ttf-2.0.14/lib/x86/SDL2_ttf.lib;C:/Users/ndt59/OneDrive - University of Suffolk/UOS/Units/Networking/Code/SDLGameNetworking/Libraries/SDL2_image-2.0.4/lib/x86/SDL2_image.lib"
  IMPORTED_LOCATION_MINSIZEREL "${_IMPORT_PREFIX}/bin/KiWi.dll"
  )

list(APPEND _IMPORT_CHECK_TARGETS KiWi )
list(APPEND _IMPORT_CHECK_FILES_FOR_KiWi "${_IMPORT_PREFIX}/lib/KiWi.lib" "${_IMPORT_PREFIX}/bin/KiWi.dll" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
