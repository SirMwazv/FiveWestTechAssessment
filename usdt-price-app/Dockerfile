# Stage 1: Build the React app
FROM node:16-alpine AS build
WORKDIR /app

# Copy the package.json and install dependencies
COPY package.json ./
RUN npm install

# Copy the rest of the application code
COPY . ./

# Build the React app
RUN npm run build

# Stage 2: Serve the app with NGINX
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html

# Expose the port NGINX is running on
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]
