using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;

public static class Checksum
{
//User Input Variables
    public static string pathraw;
    public static string hashraw;
    public static string typeraw;
    public static string lengthraw;
//cleaned variables
    public static string filepath;
    public static string check;
    public static string checktype;
    public static string filehash;
    public static int hashlength;
//Outputs
    public static string hash_error;
    public static string[] file_error;
//Global Booleans
    public static bool can_run = true;
    public static bool is_type_valid;
    public static bool is_check_valid;
    public static bool is_path_valid;
    public static bool is_length_valid = true;
    public static bool is_safe;
    public static bool is_type_empty;
    public static bool is_path_empty;
    public static bool is_hash_empty;
    public static bool is_length_empty;
    public static bool is_comparing;
    public static bool hashed;

//Other

//Error Handling
    private static string path_error;

    private static string[] hashes = ["MD5", "SHA1", "SHA256", "SHA384", "SHA512", "SHA3_256", "SHA3_384", "SHA3_512", "SHAKE128", "SHAKE256"];

    public static void InputCheck(){
        is_length_valid = true;
        IsValidSum();
        IsValidType();
        IsValidPath();
        IsValidLength();
    }


    public static void Start(){
        filehash = "";
        hashed = false;
  
        Certify();
        can_run = true;
    }


    public static void IsValidPath(){
        filepath = @pathraw.Replace("\"", "");
        if(filepath == ""){
            is_path_empty = true;
        }
        else{
            is_path_empty = false;
        }
        try{
            string stream = File.OpenRead(filepath).ToString();
            is_path_valid = true;
            }
        catch(Exception e){
            path_error = e.ToString();
            is_path_valid = false;
        }
        file_error = path_error.Split(new string[] {"\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

//cleans up imperfecions and checks if hashtype is valid
    public static void IsValidType(){

        //cleanup
        checktype = typeraw.Replace(" ", "");
        checktype = checktype.Replace("-", "");
        checktype = checktype.Replace("\"", ""); // the backslash is an escape character
        checktype = @checktype.ToUpper();

        //confirm validity
        if(checktype == ""){
            is_type_empty = true;
        }
        else{
            is_type_empty = false;
        }
        if(hashes.Contains(checktype)){
            is_type_valid = true;
        }
        else{
            is_type_valid = false;
        }
    }

    public static void IsValidSum(){
        check = hashraw.Replace("\"", "");
        check = hashraw.Replace("-","");
        if(check == ""){
            is_hash_empty = true;
        }
        else{
            is_hash_empty = false;
        }
        if(Regex.IsMatch(check, @"^[a-zA-Z0-9]+$")){
            is_check_valid = true;
            is_comparing = true;
        }
        else if(check == "!"){
            is_comparing = false;
            is_check_valid = true;
        }
        else{
            is_check_valid = false;
        }
}

    public static void IsValidLength(){
        if(checktype == "SHAKE128" || checktype == "SHAKE256"){
        if(is_comparing){
            hashlength = check.Length;
            is_length_valid = true;
            is_length_empty = false;
        }
        else if(lengthraw != ""){
            is_length_empty = false;
            lengthraw = lengthraw.Replace(" ", "");
            if(Regex.IsMatch(lengthraw, @"^\d+$")){
                hashlength = lengthraw.ToInt();
                is_length_valid = true;
            }
            else{
                is_length_valid = false;
            }
        }
        else{
            is_length_empty = true;
            is_length_valid = false;
        }
        }
    }
    public static void Certify(){
        if(checktype == "MD5"){
            MD5(filepath);
        }
        else if(checktype == "SHA1"){
            SHA1(filepath);
        }
        else if(checktype == "SHA256"){
            SHA256(filepath);
        }
        else if(checktype == "SHA384"){
            SHA384(filepath);
        }
        else if(checktype == "SHA3_256"){
            SHA3_256(filepath);
        }
        else if(checktype == "SHA3_384"){
            SHA3_384(filepath);
        }
        else if(checktype == "SHA3_512"){
            SHA3_512(filepath);
        }
        else if(checktype == "SHA512"){
            SHA512(filepath);
        }
        else if(checktype == "SHAKE128"){
            Shake128(filepath);
        }
        else if(checktype == "SHAKE256"){
            Shake256(filepath);
        }
        else{
            UI.UnknownErrorOccurred();
        }

    }



    public static void ChecksumOutput(){
        if(hashed){
            if(string.Equals(check, filehash, StringComparison.OrdinalIgnoreCase))
            {
                is_safe = true;
            }
            else{
                is_safe = false;
            }
            can_run = true;
        }
        else{
            UI.ErrorOccurred();
        }
        UI.WriteOutput();
    }

    public static void MD5(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.MD5.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA1(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA1.Create())
        {
            using(var stream = File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA256(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA256.Create())
        {
            using(var stream = File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA384(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA384.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA3_256(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA3_256.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;    
            ChecksumOutput();
        }

    public static void SHA3_384(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA3_384.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA3_512(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA3_512.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void SHA512(string filename){
        try{
        using(var encrypt = System.Security.Cryptography.SHA512.Create())
        {
            using(var stream = System.IO.File.OpenRead(filename))
            {
                var hashtemp = encrypt.ComputeHash(stream);
                filehash = BitConverter.ToString(hashtemp).Replace("-","");
            }
        }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void Shake128(string filename){
        try{
            using(var stream = System.IO.File.OpenRead(filename)){
                byte[] hashtemp = System.Security.Cryptography.Shake128.HashData(stream,hashlength/2);
                filehash = BitConverter.ToString(hashtemp).Replace("-", "");
            }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }

    public static void Shake256(string filename){
        try{
            using(var stream = System.IO.File.OpenRead(filename)){
                byte[] hashtemp = System.Security.Cryptography.Shake256.HashData(stream,hashlength/2);
                filehash = BitConverter.ToString(hashtemp).Replace("-", "");
            }
        }
        catch(Exception e)
        {
            hash_error = e.ToString();
            UI.ErrorOccurred();
        }
            hashed = true;
            ChecksumOutput();
        }


}