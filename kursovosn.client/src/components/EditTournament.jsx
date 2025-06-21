import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

const EditTournament = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [tournament, setTournament] = useState(null);
    const [sports, setSports] = useState([]);
    const [formats, setFormats] = useState([]);

    useEffect(() => {
        axios.get(`http://localhost:5082/api/tournament/${id}`)
            .then(res => setTournament(res.data))
            .catch(err => console.error("Ошибка загрузки турнира:", err));

        axios.get('http://localhost:5082/api/teams/sports')
            .then(res => setSports(res.data.$values))
            .catch(err => console.error("Ошибка загрузки видов спорта:", err));

        axios.get('http://localhost:5082/api/tournament/formats')
            .then(res => setFormats(res.data.$values))
            .catch(err => console.error("Ошибка загрузки форматов:", err));
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem("token");
            await axios.put(`http://localhost:5082/api/tournament/${id}`, tournament, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Турнир успешно обновлен");
            navigate(`/tournaments/${id}`);
        } catch (error) {
            console.error("Ошибка при сохранении:", error);
            alert("Ошибка при обновлении турнира");
        }
    };

    if (!tournament) return <p>Загрузка...</p>;

    return (
        <div style={{ maxWidth: '700px', margin: 'auto' }}>
            <h2>Редактировать турнир</h2>
            <form onSubmit={handleSubmit}>
                <label>Название:</label>
                <input value={tournament.name} onChange={e => setTournament({ ...tournament, name: e.target.value })} required />

                <label>Адрес:</label>
                <input value={tournament.adress} onChange={e => setTournament({ ...tournament, adress: e.target.value })} />

                <label>Возраст:</label>
                <input value={tournament.age} onChange={e => setTournament({ ...tournament, age: e.target.value })} />

                <label>Пол:</label>
                <select value={tournament.pol} onChange={e => setTournament({ ...tournament, pol: e.target.value })}>
                    <option value="Мужской">Мужской</option>
                    <option value="Женский">Женский</option>
                    <option value="Смешанный">Смешанный</option>
                </select>

                <label>Дата начала:</label>
                <input type="date" value={tournament.start.slice(0, 10)} onChange={e => setTournament({ ...tournament, start: e.target.value })} />

                <label>Дата окончания:</label>
                <input type="date" value={tournament.end.slice(0, 10)} onChange={e => setTournament({ ...tournament, end: e.target.value })} />

                <label>Информация:</label>
                <textarea value={tournament.info} onChange={e => setTournament({ ...tournament, info: e.target.value })} />

                <label>Формат:</label>
                <select value={tournament.format_ID} onChange={e => setTournament({ ...tournament, format_ID: parseInt(e.target.value) })}>
                    {formats.map(f => (
                        <option key={f.id} value={f.id}>{f.name}</option>
                    ))}
                </select>

                <label>Вид спорта:</label>
                <select value={tournament.sport_ID} onChange={e => setTournament({ ...tournament, sport_ID: parseInt(e.target.value) })}>
                    {sports.map(s => (
                        <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                </select>

                <label>Одиночный турнир:</label>
                <input type="checkbox" checked={tournament.solo} onChange={e => setTournament({ ...tournament, solo: e.target.checked })} />

                <br />
                <button type="submit">Сохранить</button>
            </form>
        </div>
    );
};

export default EditTournament;
