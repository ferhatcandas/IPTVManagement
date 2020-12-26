FROM node:12.18-alpine
ENV NODE_ENV=production
ENV PORT=9090
WORKDIR /usr/src/app
COPY ["package.json", "package-lock.json*", "npm-shrinkwrap.json*", "./"]
RUN npm install --production --silent 
COPY . .
EXPOSE 9090
CMD ["npm", "start"]
