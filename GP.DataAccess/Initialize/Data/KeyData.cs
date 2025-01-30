using GP.Core.Enums;
using GP.Domain.Entities.Lang;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<Key> BuildKeyList()
        {
            var az = LanguageStringEnum.az_AZ;
            var en = LanguageStringEnum.en_US;

            var list = new List<Key>()
            {
                new Key()
                {
                    Label = "auth",
                    Children = new List<Key>()
                    {
                        new Key()
                        {
                            Label = "username",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Istifadəçi adı"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Username"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "password",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Şifrə"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Password"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "oldPassword",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Köhnə Şifrə"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Old Password"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "confirmPassword",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Şifənin təkrarı"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Confirm password"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "resetPassword",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Şifrəni bərpa et"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Restore password"
                                },
                            }
                        }
                    }
                }
            };

            return list;
        }
    }
}
