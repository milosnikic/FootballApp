FROM node:14.9-alpine
LABEL author="Milos Nikic"

# set working directory
RUN mkdir /usr/share/app
WORKDIR /usr/share/app

# install and cache app dependencies
COPY package.json package.json
RUN npm i -g @angular/cli@8.3.29
COPY package-lock.json package-lock.json
RUN npm i

# add the app
COPY . .

# add `node_modules/.bin` to $PATH
ENV PATH /node_modules/.bin:$PATH
EXPOSE 80
EXPOSE 9222
CMD ["ng", "serve", "--host=0.0.0.0", "--watch", "--poll=2000"]