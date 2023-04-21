FROM node:19-alpine

WORKDIR /App

COPY ./package.json .
COPY ./yarn.lock .
RUN yarn install --fronzen-lockfile

COPY . .
RUN yarn run build

ENV NODE_ENV production
EXPOSE 80
CMD [ "yarn", "run", "dev" ]
