FROM node:14 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . ./

RUN npm run build

FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

COPY --from=build /app/dist .

ENTRYPOINT ["nginx", "-g", "daemon off;"]
