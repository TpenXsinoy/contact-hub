FROM node:16

WORKDIR /app

COPY package.json .

RUN npm cache clean --force

RUN npm install --legacy-peer-deps

COPY . .

EXPOSE 3000 

# EXPOSE 80 this port is for production

CMD ["npm", "run", "dev"]
