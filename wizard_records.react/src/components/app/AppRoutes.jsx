import AboutUs from "../Aboutus";
import ArtistAlbums from "../ArtistAlbums";
import ContactForm from "../ContactUs";
import Detail from "../Detail";
import Home from "../Home";
import CategoryResults from "../CategoryResults";
import SearchResults from "../SearchResults";
import ProductGallery from "../ProductGallery";
import AddProductForm from "../AddProductForm";
import Cart from "../Cart";
import Account from "../Account";
import OrderPage from "../OrderPage";
import PreviousOrdersPage from "../PreviousOrdersPage";
import CheckoutScreen from "../CheckoutScreen";
import Confirmation from "../Confirmation";
import PaymentFailed from "../Falied";
import History from "../History";


const AppRoutes = [
    {
        path: '/',
        element: <Home />,
        exact: true
    },
    {
        path: '/detail/:id',
        element: <Detail />
    },
    {
        path: '/aboutus',
        element: <AboutUs />
    },
    {
        path: '/contactus',
        element: <ContactForm />
    },
    {
        path: '/category',
        element: <CategoryResults />
    },
    {
        path: '/search',
        element: <SearchResults />
    },
    {
        path: '/add-product',
        element: <AddProductForm />
    },
    {
        path: '/products',
        element: <ProductGallery />
    },
    {
        path: '/artist/:artistName',
        element: <ArtistAlbums /> 
    },
    {
        path: '/account',
        element: <Account />
    },
    {

        path: '/cart',
        element: <Cart />
	},
	{
        path: '/order',
        element: <OrderPage />
    },
    {
        path: '/previous_orders',
        element: <PreviousOrdersPage />
    },
    {
        path: '/checkout/:orderId',
        element: <CheckoutScreen />
    },
    {
        path: '/confirmation',
        element: <Confirmation />
    },
    {
        path: '/failed/:orderId',
        element: <PaymentFailed />
    },
    {
        path: '/history',
        element: <History />
    }
  


];

export default AppRoutes;
