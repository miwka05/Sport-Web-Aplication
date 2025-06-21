// TeamForm.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';

const TeamForm = () => {
    const [name, setName] = useState('');
    const [city, setCity] = useState('');
    const [age, setAge] = useState(0);
    const [sports, setSports] = useState([]);
    const [sportId, setSportId] = useState(null);

    useEffect(() => {
        axios.get('http://localhost:5082/api/teams/sports')
            .then(res => setSports(res.data.$values))
            .catch(err => console.error('Ошибка загрузки видов спорта:', err));
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem("token");
            await axios.post('http://localhost:5082/api/teams/create', {
                name,
                city,
                age,
                sport_ID: sportId
            }, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Команда создана!");
        } catch (error) {
            console.error("Ошибка создания команды:", error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input type="text" placeholder="Название" value={name} onChange={(e) => setName(e.target.value)} required />
            <input type="text" placeholder="Город" value={city} onChange={(e) => setCity(e.target.value)} required />
            <input type="number" placeholder="Возраст" value={age} onChange={(e) => setAge(Number(e.target.value))} required />
            <select value={sportId || ''} onChange={(e) => setSportId(Number(e.target.value))} required>
                <option value="">Выберите вид спорта</option>
                {sports.map(s => (
                    <option key={s.id} value={s.id}>{s.name}</option>
                ))}
            </select>
            <button type="submit">Создать</button>
        </form>
    );
};

export default TeamForm;
