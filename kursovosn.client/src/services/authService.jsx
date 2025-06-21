// authService.js
import axios from 'axios';

const API_URL = 'http://localhost:5082/api/auth/';  // Укажите свой API URL

export const registerUser = async (userData) => {
    try {
        const response = await axios.post('http://localhost:5082/api/auth/register', userData);

        // Проверяем, что ответ от сервера валиден
        if (response && response.data) {
            return response.data;  // Успешный ответ от сервера
        }
        throw new Error('Empty response from server');
    } catch (error) {
        console.error('Error during registration:', error.response ? error.response.data : error);
        throw error;  // Прокидываем ошибку дальше
    }
};

export const loginUser = async (userData) => {
    try {
        const response = await axios.post('http://localhost:5082/api/auth/login', userData);

        // Проверяем, что ответ от сервера валиден
        if (response && response.data) {
            sessionStorage.setItem("token", response.data.token);
            return response.data;  // Успешный ответ от сервера
        }
        throw new Error('Empty response from server');
    } catch (error) {
        console.error('Error during login:', error.response ? error.response.data : error);
        throw error;  // Прокидываем ошибку дальше
    }
};

export const logoutUser = async() => {
    sessionStorage.removeItem("token");
    window.location.href = '/login';
};

export const authService = {
    getCurrentUser: async () => {
        const token = sessionStorage.getItem("token");
        const response = await axios.get('http://localhost:5082/api/auth/me', {
            headers: { Authorization: `Bearer ${token}` } // OK
        });
        return response.data;
    }
};



