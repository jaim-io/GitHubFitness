FROM node:19-alpine

WORKDIR /App

COPY ./package.json .
COPY ./package-lock.json .
RUN yarn install --fronzen-lockfile

COPY . .
RUN yarn run build

ENV NODE_ENV production
EXPOSE 3000
CMD [ "yarn", "run", "dev" ]
