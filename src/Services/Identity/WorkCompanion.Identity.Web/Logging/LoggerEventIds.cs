namespace WorkCompanion.Identity.Web.Logging;

/// <summary>
///     Static class that exposes logging event ids.
/// </summary>
public static class LoggerEventIds
{
    /// <summary>
    ///     Event id when a user is created by an external provider.
    /// </summary>
    public static EventId UserCreatedByExternalProvider => new(1, "UserCreatedByExternalProvider");

    /// <summary>
    ///     Event id when a user is logged in by an external provider.
    /// </summary>
    public static EventId UserLoggedInByExternalProvider =>
        new(2, "UserLoggedInByExternalProvider");

    /// <summary>
    ///     Event id when a user is logged in.
    /// </summary>
    public static EventId UserLogin => new(3, "UserLogin");

    /// <summary>
    ///     Event id when a user is locked out.
    /// </summary>
    public static EventId UserLockout => new(4, "UserLockout");

    /// <summary>
    ///     Event id when a user is logged in two factor authentication.
    /// </summary>
    public static EventId UserLoginWith2FA => new(5, "UserLoginWith2FA");

    /// <summary>
    ///     Event id when a user has entered an invalid authenticator code.
    /// </summary>
    public static EventId InvalidAuthenticatorCode => new(6, "InvalidAuthenticatorCode");

    /// <summary>
    ///     Event id when a user is logged in with recovery code.
    /// </summary>
    public static EventId UserLoginWithRecoveryCode => new(7, "UserLoginWithRecoveryCode");

    /// <summary>
    ///     Event id when a user has entered an invalid recovery code.
    /// </summary>
    public static EventId InvalidRecoveryCode => new(8, "InvalidRecoveryCode");

    /// <summary>
    ///     Event id when a user has changed the password.
    /// </summary>
    public static EventId PasswordChanged => new(9, "PasswordChanged");

    /// <summary>
    ///     Event id when a user has been deleted.
    /// </summary>
    public static EventId UserDeleted => new(10, "UserDeleted");

    /// <summary>
    ///     Event id when a user has disabled two factor authentication.
    /// </summary>
    public static EventId TwoFADisabled => new(11, "TwoFADisabled");

    /// <summary>
    ///     Event id when a user has requested the personal data.
    /// </summary>
    public static EventId PersonalDataRequested => new(12, "PersonalDataRequested");

    /// <summary>
    ///     Event id when a user has enabled two factor authentication.
    /// </summary>
    public static EventId TwoFAEnabled => new(13, "TwoFAEnabled");

    /// <summary>
    ///     Event id when two factor authentication recovery code is generated.
    /// </summary>
    public static EventId TwoFARecoveryGenerated => new(14, "TwoFARecoveryGenerated");

    /// <summary>
    ///     Event id when user has reset the authentication app key.
    /// </summary>
    public static EventId AuthenticationAppKeyReset => new(15, "AuthenticationAppKeyReset");

    /// <summary>
    ///     Event id when a user is created.
    /// </summary>
    public static EventId UserCreated => new(16, "UserCreated");

    /// <summary>
    ///     Event id when a user is logged out.
    /// </summary>
    public static EventId UserLoggedOut => new(17, "UserLoggedOut");
}
