# Use the official Nginx image
FROM nginx:latest

# Copy the Nginx configuration into the container
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80 for Nginx
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]