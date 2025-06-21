// Login.js
import React, { useState } from 'react';
import { loginUser } from '../services/authService'; // Импорт сервиса

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();

        const userData = {
            username: username,
            password: password
        };

        try {
            await loginUser(userData);
            window.location.href = '/list';
        } catch (err) {
            setError('Login failed: ' + err.response?.data?.message || err.message);
        }
    };

    return (
        <form onSubmit={handleLogin}>
            <div>
                <label>Username:</label>
                <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div>
                <label>Password:</label>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            {error && <p>{error}</p>}
            <button type="submit">Login</button>
        </form>
    );
};

export default Login;
