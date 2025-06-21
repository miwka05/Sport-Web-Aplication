import axios from 'axios';

const API_URL = 'http://localhost:5082/api';

export const userService = {
    updateProfile: async (data) => {
        const token = sessionStorage.getItem("token");
        return await axios.put(`${API_URL}/auth/update-profile`, data, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
    },
};
