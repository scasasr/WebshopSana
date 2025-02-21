import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

export const productsApi = createApi({
  reducerPath: "productsApi",
  baseQuery: fetchBaseQuery({ baseUrl: "https://localhost:7270/api/" }),
  endpoints: (builder) => ({
    getPaginatedProducts: builder.query({
      query: (page) => `Product/GetPaginated?page=${page}`,
      transformResponse: (response) => ({
        data: response.data,
        totalPages: response.totalPages,
      }),
    }),
  }),
});

export const { useGetPaginatedProductsQuery } = productsApi;