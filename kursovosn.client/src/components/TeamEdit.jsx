import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

const TeamEdit = () => {
    const { id } = useParams();
    const [team, setTeam] = useState(null);
    const [sports, setSports] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const token = sessionStorage.getItem("token");
        axios.get(`http://localhost:5082/api/teams/${id}`)
            .then(res => setTeam(res.data))
            .catch(err => console.error("Ошибка загрузки команды", err));

        axios.get(`http://localhost:5082/api/teams/sports`)
            .then(res => setSports(res.data.$values))
            .catch(err => console.error("Ошибка загрузки видов спорта", err));
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = sessionStorage.getItem("token");
            await axios.put(`http://localhost:5082/api/teams/${id}/edit`, {
                name: team.name,
                city: team.city,
                age: team.age,
                sport_ID: team.sport_ID
            }, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Команда успешно обновлена");
            navigate(`/team/${id}`);
        } catch (error) {
            console.error("Ошибка при сохранении изменений", error);
            alert("Ошибка обновления");
        }
    };

    if (!team) return <p>Загрузка...</p>;

    return (
        <div style={{ maxWidth: '600px', margin: 'auto' }}>
            <h2>Редактировать команду</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Название:</label>
                    <input
                        type="text"
                        value={team.name}
                        onChange={(e) => setTeam({ ...team, name: e.target.value })}
                        required
                    />
                </div>
                <div>
                    <label>Город:</label>
                    <input
                        type="text"
                        value={team.city}
                        onChange={(e) => setTeam({ ...team, city: e.target.value })}
                        required
                    />
                </div>
                <div>
                    <label>Возраст:</label>
                    <input
                        type="number"
                        value={team.age}
                        onChange={(e) => setTeam({ ...team, age: parseInt(e.target.value) })}
                        required
                    />
                </div>
                <div>
                    <label>Вид спорта:</label>
                    <select
                        value={team.sport_ID}
                        onChange={(e) => setTeam({ ...team, sport_ID: parseInt(e.target.value) })}
                    >
                        {sports.map(s => (
                            <option key={s.id} value={s.id}>{s.name}</option>
                        ))}
                    </select>
                </div>
                <button type="submit">Сохранить</button>
            </form>
        </div>
    );
};

export default TeamEdit;
