# e-Compras API
Api que serve os dados para o aplicativo e-Compras.


#### Push Notifications 🔔
Para disparar uma notificação para os usuários do e-Compras, envie uma requisição do tipo *POST* para
> /notificacoes/requests

Com o corpo

```json
{
  "texto": "MENSAGEM_AQUI",
  "numeroLicitacao": "CODIGO_FORMATADO_AQUI",
  "codigoLicitacao": "CODIGO_DA_LICITACAO_AQUI",
  "action": "alteracao_anexo",
}
```

PS: Todas as requisições feitas para esta API precisam ter no cabeçalho;
> X-API-KEY: [CHAVE_DA_API]
