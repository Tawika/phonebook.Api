# phonebook.Api

Open a terminal and run the following:

### git clone https://github.com/Tawika/phonebook.Api.git

### cd phonebook.Api

### docker build -t phonebookapi-image .

NB .... Once image has built successfully, ensure that no other processes are using port 80 for example IIS before building and running the container.

### docker run -d --name phonebook-container -p 80:80 phonebookapi-image

Ensure the api is running by navigating to http://localhost/api/phonebook. Should return an empty array.
