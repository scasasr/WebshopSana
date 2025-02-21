import { useDispatch, useSelector } from "react-redux";
import { useEffect, useState } from "react";
import { useGetPaginatedProductsQuery } from "../slices/productsApi";
import { addToCart, getTotals } from "../slices/cartSlice";
import placeholderImage from "../assets/images/placeholder.png"; 


const Home = () => {
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();
  const [page, setPage] = useState(1);
  const { data, error, isLoading } = useGetPaginatedProductsQuery(page);

  useEffect(() => {
      dispatch(getTotals());
    }, [cart, dispatch]);
  
  
  const handleAddToCart = (product) => {
    dispatch(addToCart({ ...product, cartQuantity: 1 }));
  };

  return (
    <div className="home-container">
      {isLoading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>Error loading products</p>
      ) : (
        <>
          <h2>Products</h2>
          <div className="products">
            {data?.data?.map((product) => (
              <div key={product.id} className="product">
                {/* 🖼 Imagen del producto */}
                <img src={product.image || placeholderImage} alt={product.name} className="product-image" />

                <div className="details">
                  <h3>{product.name}</h3>
                  <p className="product-code">Code: {product.referenceCode}</p>
                  <p className="stock">{product.stock} in stock</p>
                  <p className="description">
                    {product.description.length > 30
                      ? `${product.description.substring(0, 30)}...`
                      : product.description}
                  </p>
                </div>

                <div className="price-actions">
                  <span className="price">${product.price.toFixed(2)}</span>
                  <button className="add-to-cart-btn small-btn" onClick={() => handleAddToCart(product)}>
                    Add To Cart
                  </button>
                </div>
              </div>
            ))}
          </div>

          <div className="pagination">
            <button onClick={() => setPage(page - 1)} disabled={page === 1} className="pagination-btn">
              Previous
            </button>
            <span>Page {page} of {data?.totalPages}</span>
            <button onClick={() => setPage(page + 1)} disabled={page >= data?.totalPages} className="pagination-btn">
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
};

export default Home;
