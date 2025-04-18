
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Godot;

public static class Checksum
{
//User Input Variables
    public static string pathraw;
    public static string hashraw;
    public static string typeraw;
    public static string filepath;
    public static string hash;
    public static string checktype;
//Outputs

//Global Booleans
    public static bool can_run = true;

    public static bool is_base64;
    public static bool is_base32;
    public static bool is_type_valid;
    public static bool is_check_valid;

    private static string[] hashes = ["MD2", "MD4", "MD5", "SHA1", "SHA256", "SHA384", "SHA512"];



    public static void Start(){
        PathCleanup();
        //Process will run and input will include filepath, checktype, and check
        UI.Success();
        can_run = true;
    }

    public static void InputCheck(){
        IsValidSum();
        IsValidType();
    }


    public static void PathCleanup(){
        filepath = @pathraw.Replace("\"", "");
    }
//cleans up imperfecions and checks if hashtype is valid
    public static void IsValidType(){

        //cleanup
        checktype = typeraw.Replace(" ", "");
        checktype = checktype.Replace("-", "");
        checktype = checktype.Replace("\"", ""); // the backslash is an escape character
        checktype = @checktype.ToUpper();

        //confirm validity
        if(hashes.Contains(checktype)){
            is_type_valid = true;
        }
        else{
            is_type_valid = false;
        }
    }

    public static void IsValidSum(){
        hash = hashraw.Replace("\"", "");
        string pattern256 = @"^[a-fA-F0-9]{64}$";
        is_base64 = Regex.IsMatch(hash, pattern256);

        string patternmd5 = @"^[a-fA-F0-9]{32}$";
        is_base32 = Regex.IsMatch(hash, patternmd5);
        
        if(is_base64 || is_base32){
            is_check_valid = true;
        }
        else{
            is_check_valid = false;
        }
}




}
