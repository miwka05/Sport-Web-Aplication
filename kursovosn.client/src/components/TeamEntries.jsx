import React, { useEffect, useState, useContext } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';
import { UserContext } from './UserContext';

const TeamEntries = () => {
    const { id } = useParams(); // team id
    const [entries, setEntries] = useState([]);
    const [team, setTeam] = useState(null);
    const currentUser = useContext(UserContext);

    useEffect(() => {
        fetchEntries();
        fetchTeam();
    }, []);

    const fetchEntries = async () => {
        try {
            const token = sessionStorage.getItem("token");
            const response = await axios.get(`http://localhost:5082/api/teams/${id}/entries`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setEntries(response.data.values.$values);
        } catch (error) {
            console.error('Ошибка загрузки заявок:', error);
            setEntries([]); 
        }
    };

    const fetchTeam = async () => {
        try {
            const response = await axios.get(`http://localhost:5082/api/teams/${id}`);
            setTeam(response.data);
        } catch (error) {
            console.error('Ошибка загрузки команды:', error);
        }
    };

    const handleDecision = async (entryId, decision) => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/teams/entry/${entryId}/${decision}`, null, {
                headers: { Authorization: `Bearer ${token}` }
            });
            fetchEntries();
        } catch (error) {
            console.error('Ошибка при обработке заявки:', error);
        }
    };

    if (!team || !currentUser) return <div>Загрузка...</div>;
    const isCreator = team.creatorId === currentUser?.id;
    if (!isCreator) {
        return <div>Доступ запрещён. Вы не являетесь создателем команды.</div>;
    }

    return (
        <div>
            <h2>Заявки в команду "{team.name}"</h2>
            {entries.length === 0 ? (
                <p>Заявок пока нет.</p>
            ) : (
                <ul>
                    {entries.map((entry) => (
                        <li key={entry.id}>
                            <p>
                                Пользователь:{" "}
                                <Link to={`/profile/${entry.userId}`}>{entry.username}</Link>
                            </p>
                            <p>Доп. информация: {entry.info}</p>
                            <button onClick={() => handleDecision(entry.id, 'accept')}>Принять</button>
                            <button onClick={() => handleDecision(entry.id, 'reject')}>Отклонить</button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default TeamEntries;
