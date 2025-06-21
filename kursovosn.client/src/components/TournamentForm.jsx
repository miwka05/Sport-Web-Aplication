import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const TournamentForm = () => {
    const [form, setForm] = useState({
        name: '',
        sport_ID: '',
        adress: '',
        age: '',
        info: '',
        pol: '',
        start: '',
        end: '',
        format_ID: '',
        solo: false
    });
    const navigate = useNavigate();

    const [sports, setSports] = useState([]);
    const [formats, setFormats] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5082/api/tournament/sports')
            .then(res => setSports(res.data.$values))
            .catch(err => console.error("Ошибка загрузки видов спорта:", err));

        axios.get('http://localhost:5082/api/tournament/formats')
            .then(res => setFormats(res.data.$values))
            .catch(err => console.error("Ошибка загрузки форматов:", err));
    }, []);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm({ ...form, [name]: type === 'checkbox' ? checked : value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem("token");
            if (!token) {
                alert("Вы не авторизованы. Пожалуйста, войдите в систему.");
                return;
            }

            await axios.post('http://localhost:5082/api/tournament/create-tournament', {
                ...form,
                sport_ID: parseInt(form.sport_ID),
                format_ID: parseInt(form.format_ID),
                solo: form.solo
            }, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            alert("Турнир успешно создан!");
            navigate('/');
        } catch (err) {
            console.error("Ошибка при создании турнира:", err);
            alert("Ошибка при создании турнира");
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: '500px', margin: 'auto' }}>
            <div>
                <input name="name" placeholder="Название" onChange={handleChange} required />
            </div>

            <div>
                <select name="sport_ID" onChange={handleChange} required>
                    <option value="">Выберите вид спорта</option>
                    {sports.map(s => (
                        <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                </select>
            </div>

            <div>
                <input name="adress" placeholder="Адрес" onChange={handleChange} />
            </div>

            <div>
                <label>Возрастная группа:</label>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <input
                        type="number"
                        min="0"
                        max="99"
                        placeholder="Возраст"
                        value={form.age.replace(/[+-]/, '')} // только число
                        onChange={(e) => {
                            const ageNumber = e.target.value.replace(/[^\d]/g, '');
                            const currentSign = form.age.includes('+') ? '+' : '-';
                            setForm({ ...form, age: ageNumber + currentSign });
                        }}
                        required
                    />

                    <select
                        value={form.age.includes('+') ? '+' : '-'}
                        onChange={(e) => {
                            const currentNumber = form.age.replace(/[^\d]/g, '');
                            setForm({ ...form, age: currentNumber + e.target.value });
                        }}
                        required
                    >
                        <option value="+">+</option>
                        <option value="-">-</option>
                    </select>
                </div>
                <small style={{ color: 'gray' }}>
                    Например: <strong>14+</strong> — старше 14 лет, <strong>16-</strong> — младше 16 лет
                </small>
            </div>

            <div>
                <input name="info" placeholder="Описание" onChange={handleChange} />
            </div>

            <div>
                <select name="pol" onChange={handleChange} required>
                    <option value="">Выберите пол</option>
                    <option value="Мужской">Мужской</option>
                    <option value="Женский">Женский</option>
                    <option value="Любой">Любой</option>
                </select>
            </div>

            <div>
                <input type="datetime-local" name="start" onChange={handleChange} required />
            </div>

            <div>
                <input type="datetime-local" name="end" onChange={handleChange} required />
            </div>

            <div>
                <select name="format_ID" onChange={handleChange} required>
                    <option value="">Выберите формат</option>
                    {formats.map(f => (
                        <option key={f.id} value={f.id}>{f.name}</option>
                    ))}
                </select>
            </div>

            <div>
                <label>
                    <input type="checkbox" name="solo" checked={form.solo} onChange={handleChange} />
                    Одиночный турнир
                </label>
            </div>

            <div>
                <button type="submit">Создать</button>
            </div>
        </form>
    );
};

export default TournamentForm;
