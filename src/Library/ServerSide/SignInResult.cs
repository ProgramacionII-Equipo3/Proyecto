using System;

namespace Library.ServerSide
{
    /// <summary>
    /// This enum represents potential results of attempting to sign in.
    /// </summary>
    public enum SignInResult
    {
        OkAdmin,
        OkEntrepeneur,
        OkCompany,
        NotFound,
        InvalidPassword
    }
}