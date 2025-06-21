import React, { useState } from 'react';
import axios from 'axios';

const ChangePassword = () => {
    const [form, setForm] = useState({ currentPassword: '', newPassword: '' });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");
            await axios.post('http://localhost:5082/api/auth/change-password', form, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Пароль изменен");
        } catch (error) {
            alert("Ошибка при смене пароля");
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Смена пароля</h2>
            <input name="currentPassword" type="password" placeholder="Текущий пароль" onChange={handleChange} />
            <input name="newPassword" type="password" placeholder="Новый пароль" onChange={handleChange} />
            <button type="submit">Сменить</button>
        </form>
    );
};

export default ChangePassword;
