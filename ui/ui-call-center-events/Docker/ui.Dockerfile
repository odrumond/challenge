FROM node:20 AS base

ENV PNPM_HOME="/pnpm"
ENV PATH="$PNPM_HOME:$PATH"
RUN corepack enable

FROM base AS ui-img

WORKDIR /usr/src/nuxt-app
COPY ../ .
RUN pnpm install
RUN pnpm build

EXPOSE 3000 

CMD ["pnpm", "run", "start"]