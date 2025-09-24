FROM node:20-alpine as build

ARG ENV=prd
ENV BUILD_ENV=$ENV

WORKDIR /app

COPY . .

RUN npm install --legacy-peer-deps -f
RUN echo "Building for environment: $BUILD_ENV"
RUN npm run build -- --configuration $BUILD_ENV

FROM nginx:alpine

COPY --from=build /app/dist/any-point/browser /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

