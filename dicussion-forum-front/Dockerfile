FROM node:22 AS build

# Set working directory for all build stages.
WORKDIR /app

# Copy package.json so that package manager commands can be used.
COPY package.json package-lock.json ./

RUN npm install

# Copy the rest of the source files into the image.
COPY . .

#Build the app
RUN npm run build

EXPOSE 5173

# Run the application.
CMD ["npm", "run", "dev"]