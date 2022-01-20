FROM node:10
RUN mkdir -p /app
WORKDIR /app
COPY package.json /app
RUN npm install
RUN npm install -g @angular/cli@8
COPY . /app
EXPOSE 4200/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh