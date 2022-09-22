namespace CHESF.COMPRAS.Domain.APP
{
    public class PushTemplates
    {
        public class Generic
        {
            public const string Android =
                "{ \"notification\": { \"title\" : \"e-Compras\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"action\" : \"$(alertAction)\", \"licitacao\" : \"$(licitacao)\" } }";

            public const string iOS =
                "{ \"aps\" : { \"title\" : \"e-Compras\"}, \"alert\" : \"$(alertMessage)\"}, \"action\" : \"$(alertAction)\", \"licitacao\" : \"$(licitacao)\" }";
        }

        public class Silent
        {
            public const string Android =
                "{ \"data\" : {\"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\", \"licitacao\" : \"$(licitacao)\"} }";

            public const string iOS =
                "{ \"aps\" : {\"content-available\" : 1, \"apns-priority\": 5, \"sound\" : \"\", \"badge\" : 0}, \"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\", , \"licitacao\" : \"$(licitacao)\" }";
        }
    }
}