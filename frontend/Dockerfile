FROM node:20.17.0 as build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --prod

FROM nginx:1.21.3-alpine
COPY --from=build /app/dist/frontend /usr/share/nginx/html

RUN echo\
'server {\
    listen       80;\
    listen  [::]:80;\
    server_name  localhost;\
    location / {\
        root   /usr/share/nginx/html/browser;\
        try_files $uri $uri/ /index.html;\
        index  index.html index.htm;\
    }\
    error_page   500 502 503 504  /50x.html;\
    location = /50x.html {\
        root   /usr/share/nginx/html/browser;\
    }\
}' > /etc/nginx/conf.d/default.conf

