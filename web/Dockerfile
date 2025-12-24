ARG ENVIRONMENT=Development

FROM node:20.17.0 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

ARG ENVIRONMENT
RUN if [ "$ENVIRONMENT" = "Production" ]; then \
      npm run build -- --configuration=production; \
    else \
      npm run build -- --configuration=development; \
    fi

FROM nginx:1.21.3-alpine

ARG ENVIRONMENT

COPY --from=build /app/dist/web/browser /usr/share/nginx/html

COPY default.conf /etc/nginx/conf.d/default.conf
COPY prod.conf /etc/nginx/conf.d/prod.conf

RUN if [ "$ENVIRONMENT" = "Production" ]; then \
      cp /etc/nginx/conf.d/prod.conf /etc/nginx/conf.d/default.conf; \
    fi
