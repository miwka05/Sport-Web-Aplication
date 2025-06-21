import React, { useState } from 'react';
import { registerUser } from '../services/authService'; // Импорт сервиса
import DatePicker from "react-datepicker";

const Register = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [firstName, setfirstName] = useState('');
    const [lastName, setlastName] = useState('');
    const [email, setemail] = useState('');
    const [date, setdate] = useState('');
    const [sex, setsex] = useState('');
    const [error, setError] = useState('');

    const handleRegister = async (e) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        const userData = {
            username: username,
            password: password,
            firstName: firstName,  // Убедитесь, что передаете firstName
            lastName: lastName,    // Убедитесь, что передаете lastName
            email: email,           // Если email является обязательным полем
            date: date,
            sex: sex
        };

        try {
            await registerUser(userData);  // Метод для регистрации
            window.location.href = '/login';  // После успешной регистрации перенаправляем на страницу логина
        } catch (err) {
            setError('Registration failed: ' + err.response.data.message);
        }
    };

    return (
        <form onSubmit={handleRegister}>
            <div>
                <label>Username:</label>
                <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div>
                <label>Password:</label>
                <input type="password" autoComplete="new-password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div>
                <label>Confirm Password:</label>
                <input type="password" autoComplete="new-password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
            </div>
            <div>
                <label>firstName:</label>
                <input type="text" autoComplete="firstName" value={firstName} onChange={(e) => setfirstName(e.target.value)} />
            </div>
            <div>
                <label>lastName:</label>
                <input type="text" autoComplete="lastName" value={lastName} onChange={(e) => setlastName(e.target.value)} />
            </div>
            <div>
                <label>email:</label>
                <input type="email" autoComplete="email" value={email} onChange={(e) => setemail(e.target.value)} />
            </div>
            <div>
                <label>date:</label>
                <input type="date" value={date} onChange={(e) => setdate(e.target.value)} />
            </div>
            <div>
                <label>Пол:</label>
                <select value={sex} onChange={(e) => setsex(e.target.value)} required>
                    <option value="">Выберите пол</option>
                    <option value="Мужской">Мужской</option>
                    <option value="Женский">Женский</option>
                </select>
            </div>
            {error && <p>{error}</p>}
            <button type="submit">Register</button>
        </form>
    );
};

export default Register;
