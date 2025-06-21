// UserContext.jsx
import { createContext, useEffect, useState } from 'react';
import { authService } from '../services/authService';

export const UserContext = createContext(null);

export const UserProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const loadUser = async () => {
            try {
                const userData = await authService.getCurrentUser();
                setUser(userData);
            } catch(error) { console.error("Error user context:", error); }
        };
        loadUser();
    },[]);

    return (
        <UserContext.Provider value={user}>
            {children}
        </UserContext.Provider>
    );
};

export default UserContext;
