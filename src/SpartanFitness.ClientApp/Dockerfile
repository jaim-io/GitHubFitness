# syntax=docker/dockerfile:1

# Build environment
FROM node:19-alpine as build
WORKDIR /app

COPY ./package.json .
COPY ./yarn.lock .
RUN yarn install --fronzen-lockfile

COPY . .
RUN yarn run build

# Development/Production environment
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/nginx/nginx.conf /etc/nginx/conf.d/default.conf
ENV NODE_ENV dev
EXPOSE 80
CMD [ "nginx", "-g", "daemon off;" ]