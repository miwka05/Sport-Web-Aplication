import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { UserContext } from './UserContext';

const Navbar = () => {
    const user = useContext(UserContext);

    return (
        <nav style={{ display: 'flex', justifyContent: 'space-between', padding: '1rem' }}>
            <div>
                <Link to="/" style={{ marginRight: '1rem' }}>Главная</Link>
                <Link to="/list">Турниры</Link>
            </div>
            <div>
                {user ? (
                    <Link to="/profile">Привет, {user.firstName}!</Link>
                ) : (
                    <Link to="/login">Войти</Link>
                )}
            </div>
        </nav>
    );
};

export default Navbar;

