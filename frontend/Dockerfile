FROM node:16

WORKDIR /app

COPY package.json .

RUN npm cache clean --force

RUN npm install --legacy-peer-deps

COPY . .

EXPOSE 80 

CMD ["npm", "run", "dev"]
