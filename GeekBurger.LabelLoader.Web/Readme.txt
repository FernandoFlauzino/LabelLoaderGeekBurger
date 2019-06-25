Escopo

	Com uma arquitetura baseada em micro serviços que se comunicam entre si, GeekBurger quer deixar seu 
sistema muito mais prático e rápido, para que as pessoas possam fazer seus pedidos.

LabelLoader

	1 - O LabelLoader utiliza uma ferramenta chamada Logic App do Azure que fica monitorando um Azure Blob.
	2 - Após receber uma imagem com rótulos a ferramenta faz uma requisição à API LabelLoader, 
	    que recebe a url da imagem como parâmetro e faz o download na memória e interpreta o rótulo
        utilizando o Vision Service.
	3 - Publica em uma fila do Service Bus para que a API Ingredients faça a leitura.