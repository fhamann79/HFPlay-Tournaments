using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Helpers
{
    public static class MessageHelper
    {
        public static string ModelNotValid()
        {
            return string.Format("No se pueden guardar los cambios. " +
                    "La información no es válida." +
                    "Inténtelo de nuevo, de ser el caso vuelva a cargar la fotografia respectiva " +
                    "y si el problema persiste, " +
                    "consulte al administrador del sistema.");
        }

        public static string ExceptionData()
        {
            return string.Format("No se pueden guardar los cambios. " +
                    "Se produjo una excepción en el esquema de la base de datos. " +
                    "El registro probablemente ya exista. " +
                    "Inténtelo de nuevo, de ser el caso vuelva a cargar la fotografia respectiva " +
                    "y si el problema persiste, " +
                    "consulte al administrador del sistema.");
        }

        public static string DeleteError()
        {
            return string.Format("Error al borrar. Posiblemente existan registros relacionados. Inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string NoStateFind()
        {
            return string.Format("No existe el estado del partido correspondiente. ");
        }

        internal static object MatchIsClose()
        {
            return string.Format("El partido ya se encuentra cerrado. ");
        }

        public static string DeleteErrorShort()
        {
            return string.Format("Error al borrar. ");
        }

        public static string ApproveOk()
        {
            return string.Format("Registro actualizado a aprobado exitosamente.");
        }

        public static string EditListOk()
        {
            return string.Format("Registros actualizados exitosamente.");
        }

        public static string CreateOk()
        {
            return string.Format("Registro Creado Exitosamente");
        }

        public static string NotZero()
        {
            return string.Format("El Valor no puede ser cero.");
        }

        public static string UserCreateOk()
        {
            return string.Format("Usuario Creado Exitosamente. " +
                        "Debe verificar la bandeja de entrada de su correo electrónico " +
                        "para poder iniciar sesión con su email y contraseña. " +
                        "Verifique también su bandeja de correo no deseado.");
        }

        public static string EditOk()
        {
            return string.Format("Registro Editado Exitosamente");
        }

        public static string NotApproveOk()
        {
            return string.Format("Registro actualizado a no aprobado exitosamente.");
        }

        public static string TeamFail()
        {
            return string.Format("No se encuentra el equipo");
        }

        public static string RequiredImage()
        {
            return string.Format("La imagen es requerida, de ser el caso vuelva a cargar la fotografia respectiva ");
        }

        public static string DeleteOk()
        {
            return string.Format("Registro Borrado Exitosamente");
        }

        public static string UserEditOk()
        {
            return string.Format("Usuario Editado Exitosamente");
        }

        public static string UserExits()
        {
            return string.Format("Email de usuario ya existe, de ser el caso vuelva a cargar la fotografia respectiva.");
        }

        public static string ExceptionIdentificationCardIndex()
        {
            return string.Format("Existe un registro con el mismo valor de cédula, " +
                "de ser el caso vuelva a cargar la fotografia respectiva.");
        }

        public static string ExceptionNickNameIndex()
        {
            return string.Format("Existe un registro con el mismo valor de apodo, " +
                "de ser el caso vuelva a cargar la fotografia respectiva ");
        }

        public static string NewsCreateOk()
        {
            return string.Format("Noticia creada exitosamente, se mostrará cuando sea aprobada por una administrador.");
        }

        public static string ExceptionEmailIndex()
        {
            return string.Format("Existe un registro con el mismo valor de email, " +
                "de ser el caso vuelva a cargar la fotografia respectiva ");
        }

        public static string ExceptionReference()
        {
            return string.Format("El registro tiene información relacionada. ");
        }

        public static string UserCreateFail()
        {
            return string.Format("Se produjo un error al momento de crear el " +
                "usuario asp. Inténtelo de nuevo, de ser el caso vuelva a cargar la fotografia respectiva " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string ExceptionDateTimeOutOfRange()
        {
            return string.Format("La fecha esta fuera de rango. " +
                "Ingrese una fecha mayor o igual al año 1753, de ser el caso vuelva a cargar la fotografia respectiva  " +
                "Si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string ExceptionDateNotValid()
        {
            return string.Format("La fecha no es válida. Ingrese la fecha de nacimiento correctamente, " +
                "de ser el caso vuelva a cargar la fotografia respectiva  ");
        }

        public static string AddOk()
        {
            return string.Format("Registro agregado exitosamente.");
        }

        public static string DeleteErrorASP()
        {
            return string.Format("Error al borrar usuario ASP. Posiblemente existan registros relacionados. Inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string UserTeamManagersReference()
        {
            return string.Format("Error al borrar. El Usuario es directivo de equipo. " +
                "Elimine las referencias e inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string UserTeamPlayersReference()
        {
            return string.Format("Error al borrar. El Usuario es jugador de equipo. " +
                "Elimine las referencias e inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string UserReference()
        {
            return string.Format("Error al borrar. El Usuario tiene registros relacionados. " +
                "Elimine las referencias e inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }

        public static string UserNewsItemsReference()
        {
            return string.Format("Error al borrar. El Usuario tiene publicaciones en el buzón deportivo. " +
                "Elimine las referencias e inténtelo de nuevo, " +
                "y si el problema persiste, consulte a su administrador del sistema.");
        }
    }
}